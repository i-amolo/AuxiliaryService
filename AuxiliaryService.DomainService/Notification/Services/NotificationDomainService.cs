using Adacta.AdInsure.Framework.Core.Common;
using Adacta.AdInsure.Framework.Core.Domain.Common;
using AuxiliaryService.API.AdContract;
using AuxiliaryService.API.Validation;
using AuxiliaryService.Domain.Consts;
using AuxiliaryService.Domain.Notification;
using AuxiliaryService.Domain.Notification.Consts;
using AuxiliaryService.Domain.Notification.DomainEvent;
using AuxiliaryService.Domain.Notification.Repositories;
using AuxiliaryService.Domain.Notification.Services;
using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuxiliaryService.DomainService.Notification.Services
{
	public class NotificationDomainService : DomainServiceBase, INotificationDomainService
    {
        #region private fields

        private readonly INotificationProviderResolver _providerResolver;
        private readonly INotificationRepository _repository;
        private readonly Lazy<ILog> _logger = new Lazy<ILog>(() => LogManager.GetLogger(LogConsts.SogazEmailNotification));

        #endregion 

        #region .ctor

        public NotificationDomainService(INotificationProviderResolver providerResolver,
                                         INotificationRepository repository) 
            : base(AdInsureModule.Core.ToString())
        {
            _providerResolver = providerResolver;
            _repository = repository;
        }

        #endregion

        #region  private methods

        private void SetByDefault<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            notification.Status = NotificationStatusConsts.Pending;

            var provider = _providerResolver.Resolve<TProviderDetails>(notification.ProviderType);
            provider.SetDefault(notification);
        }

        private void ValidateCreate<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {

            Validator.NotNull(() => notification);
            Validator.NotNull(() => notification.RefId);
            Validator.NotNull(() => notification.ProviderType);

            var provider = _providerResolver.Resolve<TProviderDetails>(notification.ProviderType);
            provider.Validate(notification);

        }

        private void ValidateNotify<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            if (notification.Status != NotificationStatusConsts.Pending && notification.Status != NotificationStatusConsts.Error)
            {
                throw new Exception("Notification status should be Pending");
            }
        }

        private void ValidateNotificationCriteria(NotificationCriteriaDomain criteria)
        {
            if (criteria == null ||
                ((criteria.Ids == null || !criteria.Ids.Any()) &&
                  (criteria.RefIds == null || !criteria.RefIds.Any())
                )
               )
                throw new Exception("Email notification criteria is invalid");
        }

        private NotificationDomain<TProviderDetails> SetNotificationSent<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            notification.Status = NotificationStatusConsts.Sent;
            notification.SentOn = DateTime.Now;

            _repository.SetNotificationSent(notification);

            return notification;
        }

        private NotificationDomain<TProviderDetails> SetNotificationError<TProviderDetails>(NotificationDomain<TProviderDetails> notification, Exception e) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            notification.Status = NotificationStatusConsts.Error;

            _repository.SetNotificationError(notification, e);

            return notification;
        }

        private NotificationResponseDomain Notify<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {

            AdContract.NotNull(notification);
            AdContract.NotNull(notification.Id);

            var resp = new NotificationResponseDomain() { NotificationId = notification.Id };

            try
            {

                _logger.Value.Info($"Sending [{notification.Id}].");

                ValidateNotify(notification);

                // send notification
                var provider = _providerResolver.Resolve<TProviderDetails>(notification.ProviderType);
                provider.Notify(notification);

                // set notification status
                SetNotificationSent(notification);
                resp.SentOn = notification.SentOn;

                _logger.Value.Trace($"Sent [{notification.Id}]. Notification: {JsonConvert.SerializeObject(notification)}");

            }
            catch (Exception e)
            {

                SetNotificationError(notification, e);

                _logger.Value.Error($"Sending [{notification.Id}] failed. Error: {e.ToString()} Notification: {JsonConvert.SerializeObject(notification)}");
                resp.Error = e.ToString();
            }

            return resp;
        }

        #endregion 

        #region INotificationDomainService

        public NotificationDomain<TProviderDetails> Create<TProviderDetails>(NotificationDomain<TProviderDetails> notification) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            try
            {
                _logger.Value.Info($"Creation.  Ref Id: {notification.RefId}");

                // set notification attributes by default
                SetByDefault(notification);

                // validate
                ValidateCreate(notification);

                // persist to DB
                var ntf = _repository.Save(notification);

                // raise domain event
                Raise(new NotificationDomainEvent(ntf.Id.Value, ntf.RefId, ntf.ProviderType));

                _logger.Value.Trace($"Created [{notification.Id}]. Notification: {JsonConvert.SerializeObject(notification)}");

                return ntf;

            }
            catch (Exception e)
            {
                _logger.Value.Error($"Creation failed. Error: {e.ToString()} Notification: {JsonConvert.SerializeObject(notification)}");
                throw;
            }
        }

        public IEnumerable<NotificationDomain<TProviderDetails>> GetByRef<TProviderDetails>(string refId, string providerType) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            return _repository.Get<TProviderDetails>(null, new List<string> { refId }, providerType);
        }

        public IEnumerable<NotificationResponseDomain> Notify<TProviderDetails>(NotificationCriteriaDomain criteria) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            var result = new List<NotificationResponseDomain>();

            _logger.Value.Trace($"Sending by criteria. Criteria: {JsonConvert.SerializeObject(criteria)}");

            try
            {
                ValidateNotificationCriteria(criteria);

                var notifications = _repository.Get<TProviderDetails>(criteria.Ids, criteria.RefIds, criteria.ProviderType);

                if (notifications == null || !notifications.Any())
                    _logger.Value.Trace($"No notifications found by criteria.");

                foreach (var notification in notifications)
                {
                    result.Add(Notify(notification));
                }

            }
            catch (Exception e)
            {
                _logger.Value.Error($"Sending failed. Error: {e.ToString()}");
            }

            return result;
        }

        #endregion
    }
}