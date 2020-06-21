using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Integration.Smtp
{
    public class SmtpRequest
    {
        /// <summary>
        /// from message isbeing sending
        /// correct email address
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// list of emails of recipients
        /// </summary>
        public List<string> Recipients { get; set; }

        /// <summary>
        /// list of emails to CC
        /// </summary>
        public List<string> CC { get; set; }
        
        /// <summary>
        /// message encoding
        /// default is UTF-8
        /// </summary>
        public Encoding MessageEncoding { get; set; }

        /// <summary>
        /// message subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// message Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// if body passed as HTML text
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// list of attachments link
        /// </summary>
        public IEnumerable<SmtpAttachment> Attachments { get; set; }
    }
}
