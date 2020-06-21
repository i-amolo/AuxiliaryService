using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Providers.Email.Settings
{

    public class EmailNotificationSettingsFactory : IEmailNotificationSettingsFactory
    {
        /// <summary>
        /// Create PAS integration settings class with specified provider
        /// </summary>
        /// <param name="provider">Config configuration settings provider</param>
        /// <returns>PAS integration settings</returns>
        public EmailNotificationSettings Create(IConfigConfigurationSettingsProvider provider)
        {
            return new EmailNotificationSettings(provider);
        }
    }
}
