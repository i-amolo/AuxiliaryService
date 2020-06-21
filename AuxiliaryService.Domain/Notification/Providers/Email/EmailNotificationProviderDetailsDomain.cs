using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Providers.Email
{
    public class EmailNotificationProviderDetailsDomain : NotificationProviderDetailsDomainBase
    {
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
