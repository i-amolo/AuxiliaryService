using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Notification.Email.DTO
{
    public class EmailNotificationCriteria
    {
        public List<Guid> Ids { get; set; }
        public List<string> RefIds { get; set; }
    }
}
