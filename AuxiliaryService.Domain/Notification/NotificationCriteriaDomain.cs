using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification
{
    public class NotificationCriteriaDomain
    {
        public string ProviderType { get; set; }
        public List<Guid> Ids { get; set; }
        public List<string> RefIds { get; set; }
    }
}
