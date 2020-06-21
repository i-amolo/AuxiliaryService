using AuxiliaryService.API.Shared.Notification.Email.DTO;
using AuxiliaryService.Domain.Notification;
using AuxiliaryService.Domain.Notification.Consts;
using AuxiliaryService.Domain.Notification.Providers.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.Notification.Email
{
    public static class EmailNotificationConverter
    {

        public static EmailNotification Convert(NotificationDomain<EmailNotificationProviderDetailsDomain> domainObj)
        {
            var obj = new EmailNotification()
            {
                Id = domainObj.Id,
                RefId = domainObj.RefId,
                Status = domainObj.Status,
            };

            if (domainObj.Message != null)
            {
                obj.MessageFormat = domainObj.Message.Format;
                obj.MessageSubject = domainObj.Message.Header;
                obj.MessageBody = domainObj.Message.Body;
                if (domainObj.Message.Attachments != null)
                {
                    obj.MessageAttachments = domainObj.Message.Attachments.Select(d => Convert(d)).ToList();
                }
            }

            if (domainObj.ProviderDetails != null)
            {
                obj.SmtpServerHost = domainObj.ProviderDetails.SmtpServerHost;
                obj.Port = domainObj.ProviderDetails.Port;
                obj.UserName = domainObj.ProviderDetails.UserName;
                obj.Password = domainObj.ProviderDetails.Password;
                obj.Sender = domainObj.ProviderDetails.Sender;
                obj.Recipients = domainObj.ProviderDetails.Recipients;
                obj.CC = domainObj.ProviderDetails.CC;
            }

            return obj;
        }

        public static NotificationDomain<EmailNotificationProviderDetailsDomain> Convert(EmailNotification obj)
        {
            var domainObj = new NotificationDomain<EmailNotificationProviderDetailsDomain>()
            {
                Id = obj.Id,
                RefId = obj.RefId,
                Status = obj.Status,
                ProviderType = NotificationProviderTypeConsts.Email,
                Message = new NotificationMessageDomain()
                {
                    Format = obj.MessageFormat,
                    Header = obj.MessageSubject,
                    Body = obj.MessageBody
                },
                ProviderDetails = new EmailNotificationProviderDetailsDomain()
                {
                    SmtpServerHost = obj.SmtpServerHost,
                    Port = obj.Port,
                    UserName = obj.UserName,
                    Password = obj.Password,
                    Sender = obj.Sender,
                    Recipients = obj.Recipients,
                    CC = obj.CC
                }
            };

            if (obj.MessageAttachments != null)
            {
                domainObj.Message.Attachments = obj.MessageAttachments.Select(o => Convert(o)).ToList();
            }

            return domainObj;
        }

        public static IEnumerable<EmailNotification> Convert(IEnumerable<NotificationDomain<EmailNotificationProviderDetailsDomain>> domain)
        {

            if (domain == null)
                return null;

            return domain.Select(a => Convert(a));
        }

        public static NotificationCriteriaDomain Convert(EmailNotificationCriteria obj)
        {
            return new NotificationCriteriaDomain()
            {
                ProviderType = NotificationProviderTypeConsts.Email,
                Ids = obj.Ids,
                RefIds = obj.RefIds
            };
        }

        public static IEnumerable<EmailNotificationResponse> Convert(IEnumerable<NotificationResponseDomain> domainRs)
        {
            if (domainRs == null)
                return null;

            return domainRs.Select(a => new EmailNotificationResponse() { NotificationId = a.NotificationId, SentOn = a.SentOn, Error = a.Error });
        }

        public static EmailNoticifationAttachment Convert(NotificationMessageAttachment domain)
        {
            if (domain == null)
                return null;

            return new EmailNoticifationAttachment()
            {
                EntityType = domain.EntityType,
                ConfigurationCodeName = domain.ConfigurationCodeName,
                ConfigurationCodeVersion = domain.ConfigurationCodeVersion,
                DocumentNumber = domain.DocumentNumber,
                AttachmentType = domain.AttachmentType
            };
        }

        public static NotificationMessageAttachment Convert(EmailNoticifationAttachment obj)
        {
            if (obj == null)
                return null;

            return new NotificationMessageAttachment()
            {
                EntityType = obj.EntityType,
                ConfigurationCodeName = obj.ConfigurationCodeName,
                ConfigurationCodeVersion = obj.ConfigurationCodeVersion,
                DocumentNumber = obj.DocumentNumber,
                AttachmentType = obj.AttachmentType
            };
        }
    }
}
