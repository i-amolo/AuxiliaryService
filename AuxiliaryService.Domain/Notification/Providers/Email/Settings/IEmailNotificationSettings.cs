using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Providers.Email.Settings
{

    public interface IEmailNotificationSettings : IConfigConfigurationSettings
    {
        /// <summary>
        /// smtp server 
        /// </summary>
        string SmtpServer { get; }

        /// <summary>
        /// port
        /// </summary>
        int? Port { get; }

        /// <summary>
        /// email of sender
        /// </summary>
        string SenderAddress { get; }

        /// <summary>
        /// username
        /// </summary>
        string Username { get; }

        /// <summary>
        /// password
        /// </summary>
        string Password { get; }

        /// <summary>
        /// ignored domains filter
        /// </summary>
        IEnumerable<string> IgnoredDomains { get; }
    }

}
