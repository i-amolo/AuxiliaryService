using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Notification.Email.DTO
{
    public class EmailNoticifationAttachment
    {
        public string EntityType { get; set; }
        public string ConfigurationCodeName { get; set; }
        public string ConfigurationCodeVersion { get; set; }
        public string DocumentNumber { get; set; }
        public string AttachmentType { get; set; }
    }
}
