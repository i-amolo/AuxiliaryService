using Adacta.AdInsure.Framework.Core.ApplicationServices.Common;
using Adacta.AdInsure.Framework.Core.Common;
using Adacta.AdInsure.Framework.Core.Data.Transactions;
using AuxiliaryService.API.Shared.Notification.Email;
using AuxiliaryService.API.Shared.Notification.Email.DTO;
using AuxiliaryService.Domain.Notification.Consts;
using AuxiliaryService.Domain.Notification.Providers.Email;
using AuxiliaryService.Domain.Notification.Services;
using System.Collections.Generic;

namespace AuxiliaryService.ApplicationServices.Shared.Notification.Email
{
    public class EmailNotificationService : ApplicationServiceBase, IEmailNotificationService
    {

        #region private fields

        private readonly INotificationDomainService _domainNotificationService;

        #endregion 

        #region ctor

        public EmailNotificationService(INotificationDomainService domainNotificationService)
            : base(AdInsureModule.Framework.ToString())
        {
            _domainNotificationService = domainNotificationService;
        }

        #endregion

        #region private methods

        #endregion 

        #region IEmailNotificationService

        [Transaction]
        public EmailNotification Create(EmailNotification notification)
        {
            var domainNotification = _domainNotificationService.Create(EmailNotificationConverter.Convert(notification));
            var result = EmailNotificationConverter.Convert(domainNotification);
            return result;
        }

        public IEnumerable<EmailNotification> GetByRef(string refId)
        {
            var domainResult = _domainNotificationService.GetByRef<EmailNotificationProviderDetailsDomain>(refId, NotificationProviderTypeConsts.Email);
            var result = EmailNotificationConverter.Convert(domainResult);
            return result;
        }

        [Transaction]
        public IEnumerable<EmailNotificationResponse> Notify(EmailNotificationCriteria criteria)
        {
            var domainCriteria = EmailNotificationConverter.Convert(criteria);
            var response = _domainNotificationService.Notify<EmailNotificationProviderDetailsDomain>(domainCriteria);
            return EmailNotificationConverter.Convert(response);
        }

        #endregion

    }
}
