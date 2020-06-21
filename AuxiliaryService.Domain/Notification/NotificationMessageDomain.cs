using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification
{

    public class NotificationMessageDomain
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public string Format { get; set; }
        public IEnumerable<NotificationMessageAttachment> Attachments {get;set; }
    }
}
