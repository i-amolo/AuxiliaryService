using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Notification.Email.DTO
{
    public class EmailNotificationResponse
    {
        public Guid? NotificationId { get; set; }
        public DateTime? SentOn { get; set; }
        public string Error { get; set; }
    }
}
