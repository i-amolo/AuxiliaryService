using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification
{
    public class NotificationResponseDomain
    {
        public Guid? NotificationId { get; set; }
        public DateTime? SentOn { get; set; }
        public string Error { get; set; }
    }
}
