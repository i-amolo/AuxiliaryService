using Adacta.AdInsure.Framework.Core.Web.API.Controllers;
using AuxiliaryService.API.Shared.Notification.Email;
using AuxiliaryService.API.Shared.Notification.Email.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.WebAPI.Shared.Notification.Email
{
    public class EmailNotificationServiceController : AIApiController, IEmailNotificationService
    {

        private readonly IEmailNotificationService _service;

        public EmailNotificationServiceController(IEmailNotificationService service)
        {
            _service = service;
        }

        public EmailNotification Create(EmailNotification notification)
        {
            return _service.Create(notification);
        }

        public IEnumerable<EmailNotification> GetByRef(string refId)
        {
            return _service.GetByRef(refId);
        }

        public IEnumerable<EmailNotificationResponse> Notify(EmailNotificationCriteria criteria)
        {
            return _service.Notify(criteria);
        }
    }
}
