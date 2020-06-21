using AuxiliaryService.API.Shared.Notification.Email.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace AuxiliaryService.API.Shared.Notification.Email
{
    [APIInterfaceRoutePrefix("api/shared/email-notification")]
    public interface IEmailNotificationService
    {
        /// <summary>
        /// get notifications by ref Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("get-by-ref")]
        IEnumerable<EmailNotification> GetByRef(string refId);

        /// <summary>
        /// create notification
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        EmailNotification Create(EmailNotification notification);

        /// <summary>
        /// invoke notification by criteria
        /// </summary>
        /// <param name="criteria"></param>
        [HttpPost, Route("notify")]
        IEnumerable<EmailNotificationResponse> Notify(EmailNotificationCriteria criteria);

    }
}
