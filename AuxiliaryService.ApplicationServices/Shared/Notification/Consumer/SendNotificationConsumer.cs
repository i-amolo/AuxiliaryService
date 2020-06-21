using System;
using System.Collections.Generic;
using AuxiliaryService.API.Shared.Notification.Email;
using AuxiliaryService.API.Shared.Notification.Email.DTO;
using AuxiliaryService.Domain.Notification.Consts;
using AuxiliaryService.Domain.Notification.Messages;
using Common.Logging;

namespace AuxiliaryService.ApplicationServices.Shared.Notification.Consumer
{
    public class SendNotificationConsumer : ServiceMessageConsumer<NotificationDomainEventMsg>
    {
        private static readonly ILog _logger = LogManager.GetLogger<SendNotificationConsumer>();
        private readonly IEmailNotificationService _notificationService;

        public SendNotificationConsumer(
            IDocumentSerializer documentSerializer,
            MessageQueueConfiguration messageQueueConfiguraiton,
            IServiceMessageEndpointResolver endpointResolver,
            IConsumerApplicationContextInitializer applicationContextInitializer,
            IEmailNotificationService notificationService)
            : base(documentSerializer, messageQueueConfiguraiton, endpointResolver, applicationContextInitializer)
        {
            _notificationService = notificationService;
        }

        protected override void HandleMessage(IServiceMessage<NotificationDomainEventMsg> serviceMessage)
        {
            try
            {
                _logger.Debug($"START - ServiceMessageId: {serviceMessage.ServiceMessageId}");

                var messageBody = serviceMessage.Body;

                if (messageBody.ProviderType == NotificationProviderTypeConsts.Email)
                {
                    _notificationService.Notify(new EmailNotificationCriteria() { Ids = new List<Guid> { messageBody.NotificationId } });

                    _logger.Debug($"Notification Id: {messageBody.NotificationId}, Ref Id: {messageBody.RefId}");
                }
                else
                {
                    throw new Exception($"Notification sending failed. Unsupported provider type {messageBody.ProviderType}");
                }

                _logger.Debug($"END - ServiceMessageId: {serviceMessage.ServiceMessageId}");
            }
            catch (Exception ex)
            {
                _logger.Debug(ex.ToString());

                throw;
            }
        }
    }
}