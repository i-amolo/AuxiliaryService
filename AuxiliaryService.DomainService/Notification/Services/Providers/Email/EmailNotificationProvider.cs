using AuxiliaryService.API.Shared.Integration.Smtp;
using AuxiliaryService.API.Shared.Notification.Email.Consts;
using AuxiliaryService.API.Validation;
using AuxiliaryService.Domain.Consts;
using AuxiliaryService.Domain.Notification;
using AuxiliaryService.Domain.Notification.Consts;
using AuxiliaryService.Domain.Notification.Providers.Email;
using AuxiliaryService.Domain.Notification.Providers.Email.Settings;
using AuxiliaryService.Domain.Notification.Services;
using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuxiliaryService.DomainService.Notification.Services.Providers.Email
{
	public class EmailNotificationProvider : DomainServiceBase, INotificationProvider<EmailNotificationProviderDetailsDomain>
    {
        #region private fields

        private readonly ISmtpClient _smtpClient;
        private readonly Lazy<ILog> _logger = new Lazy<ILog>(() => LogManager.GetLogger(LogConsts.EmailNotification));
        private readonly IEmailNotificationSettings _settings;
        private readonly IContractService _contractService;

        #endregion

        #region ctor

        public EmailNotificationProvider(ISmtpClient smtpClient,
                                         IEmailNotificationSettings settings,
                                         IContractService contractservice) 
            : base(Framework.ToString())
        {
            _smtpClient = smtpClient;
            _settings = settings;
            _contractService = contractservice;
        }

        #endregion

        #region INotificationProvider

        public void Notify(NotificationDomain<EmailNotificationProviderDetailsDomain> request)
        {
            SmtpClientConfiguration smtpConfiguration = CreateSmtpClientConfiguration(request);
            SmtpRequest smtpRq = CreateSmtpRequest(request);

            try
            {
                AddAttachments(request, smtpRq);

                bool anyEmailsRemoved = RemoveEmailsFromIgnoredDomains(smtpRq);
                if (anyEmailsRemoved && !smtpRq.Recipients.Any())
                {
                    _logger.Value.Info($"Notification not sent because all the recepients were removed due to {nameof(_settings.IgnoredDomains)} setting");
                    return;
                }

                _smtpClient.Send(smtpConfiguration, smtpRq);

                _logger.Value.Info($"Notification sent. Id: {request.Id} RefId: {request.RefId}");
                _logger.Value.Trace($"Request: {JsonConvert.SerializeObject(new { smtpRq.Recipients, smtpRq.Subject })}");
            }
            catch (Exception e)
            {
                _logger.Value.Error($"Failed. Notification Id: {request.Id} RefId: {request.RefId} Error: {e.ToString()}");
                _logger.Value.Error($"Configuration: {JsonConvert.SerializeObject(request)}");
                _logger.Value.Error($"Request: {JsonConvert.SerializeObject(smtpRq)}");

                throw;
            }
        }

        private bool RemoveEmailsFromIgnoredDomains(SmtpRequest smtpRq)
        {
            bool anyEmailsRemoved = false;

            if (_settings.IgnoredDomains != null && _settings.IgnoredDomains.Any())
            {
                _logger.Value.Info($"Notification {nameof(_settings.IgnoredDomains)}: {JsonConvert.SerializeObject(new { _settings.IgnoredDomains })}");

                var emailToRemoveList = (from r in smtpRq.Recipients
                                         from i in _settings.IgnoredDomains
                                         where r.EndsWith(i, StringComparison.CurrentCultureIgnoreCase)
                                         select r
                                     ).Distinct().ToList();

                anyEmailsRemoved = emailToRemoveList.Any();

                foreach (var emailToRemove in emailToRemoveList)
                {
                    _logger.Value.Info($"Email {emailToRemove} is ignored due to {nameof(_settings.IgnoredDomains)} setting");
                    smtpRq.Recipients.Remove(emailToRemove);
                }
            }

            return anyEmailsRemoved;
        }

        private static SmtpClientConfiguration CreateSmtpClientConfiguration(NotificationDomain<EmailNotificationProviderDetailsDomain> request)
        {
            return new SmtpClientConfiguration()
            {
                ServerHost = request.ProviderDetails.SmtpServerHost,
                Port = request.ProviderDetails.Port,
                UserName = request.ProviderDetails.UserName,
                Password = request.ProviderDetails.Password
            };
        }

        private static SmtpRequest CreateSmtpRequest(NotificationDomain<EmailNotificationProviderDetailsDomain> request)
        {
            return new SmtpRequest()
            {
                Sender = request.ProviderDetails.Sender,
                Recipients = request.ProviderDetails.Recipients,
                CC = request.ProviderDetails.CC,
                Subject = request.Message.Header,
                Body = request.Message.Body,
                IsBodyHtml = request.Message.Format == NotificationMessageFormatConsts.HTML
            };
        }

        private void AddAttachments(NotificationDomain<EmailNotificationProviderDetailsDomain> request, SmtpRequest smtpRq)
        {
            if (request.Message.Attachments == null)
                return;

            var attachments = new List<SmtpAttachment>();
            var contractAttachments = request.Message.Attachments.Where(a => a.EntityType == AttachmentEntityTypeConsts.Contract);
            foreach (var attachment in contractAttachments)
            {
                var response = PrintAttachment(request, smtpRq, attachment);
                if (response != null)
                {
                    attachments.Add(new SmtpAttachment
                    {
                        Name = $"{attachment.DocumentNumber}.pdf",
                        Data = response.Data,
                        MediaType = response.ContentType
                    });
                }
            }

            if (attachments.Any())
            {
                smtpRq.Attachments = attachments;
            }
        }

        private PrintResponse PrintAttachment(NotificationDomain<EmailNotificationProviderDetailsDomain> request, SmtpRequest smtpRq, NotificationMessageAttachment attachment)
        {
            try
            {
                var printRequest = new SharedDTOs.PrintRequest
                {
                    AttachmentTypes = new List<string> { attachment.AttachmentType },
                    VersionedDocumentNumber = attachment.DocumentNumber
                };
                var response = _contractService.Print(attachment.ConfigurationCodeName, attachment.ConfigurationCodeVersion, attachment.DocumentNumber, printRequest);

                return response;
            }
            catch (Exception e)
            {
                _logger.Value.Error($"Attachment printing failed. Notification Id: {request.Id} RefId: {request.RefId} Error: {e.ToString()}");
                _logger.Value.Error($"Configuration: {JsonConvert.SerializeObject(request)}");
                _logger.Value.Error($"Request: {JsonConvert.SerializeObject(smtpRq)}");
                return null;
            }
        }

        public void SetDefault(NotificationDomain<EmailNotificationProviderDetailsDomain> request)
        {
            request.ProviderDetails.SmtpServerHost = string.IsNullOrEmpty(request.ProviderDetails.SmtpServerHost) 
                ? _settings.SmtpServer
                : request.ProviderDetails.SmtpServerHost;

            request.ProviderDetails.Port = request.ProviderDetails.Port == default(int)
                ? _settings.Port.Value
                : request.ProviderDetails.Port;

            request.ProviderDetails.UserName = string.IsNullOrEmpty(request.ProviderDetails.UserName)
                ? _settings.Username
                : request.ProviderDetails.UserName;

            request.ProviderDetails.Password = string.IsNullOrEmpty(request.ProviderDetails.Password)
                ? _settings.Password
                : request.ProviderDetails.Password;

            request.ProviderDetails.Sender = string.IsNullOrEmpty(request.ProviderDetails.Sender)
                ? _settings.SenderAddress
                : request.ProviderDetails.Sender;
        }

        public void Validate(NotificationDomain<EmailNotificationProviderDetailsDomain> request)
        {
            Validator.NotNull(() => request.ProviderDetails.SmtpServerHost);
            Validator.NotNull(() => request.ProviderDetails.Sender);
            Validator.NotNull(() => request.ProviderDetails.UserName);
            Validator.NotNull(() => request.ProviderDetails.Password);
        }

        #endregion
    }
}