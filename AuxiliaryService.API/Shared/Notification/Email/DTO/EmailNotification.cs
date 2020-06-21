using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Notification.Email.DTO
{

    public class EmailNotification
    {
        /// <summary>
        /// notification id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// notification reference id
        /// </summary>
        public string RefId { get; set; }

        /// <summary>
        /// notification status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// message 
        /// </summary>
        public string MessageSubject { get; set; }

        /// <summary>
        /// message body
        /// </summary>
        public string MessageBody { get; set; }

        /// <summary>
        /// message format
        /// </summary>
        public string MessageFormat { get; set; }

        /// <summary>
        /// list of attachments
        /// </summary>
        public IEnumerable<EmailNoticifationAttachment> MessageAttachments { get; set; }

        /// <summary>
        /// smtp server
        /// </summary>
        public string SmtpServerHost { get; set; }

        /// <summary>
        /// port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        public string Password { get; set; }

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
    }


}
