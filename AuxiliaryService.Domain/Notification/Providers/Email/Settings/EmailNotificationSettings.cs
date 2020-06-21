using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using Adacta.AdInsure.Framework.Core.Localization;
using AuxiliaryService.Domain.Notification.Providers.Email.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Providers.Email.Settings
{
    public class EmailNotificationSettings : IEmailNotificationSettings
    {
        private readonly IConfigConfigurationSettingsProvider _provider;

        public EmailNotificationSettings(IConfigConfigurationSettingsProvider provider)
        {
            _provider = provider;

            var ignoredDomains = _provider.GetSetting<string>("appSettings", "AdInsure:Sogaz:Notification:Email:IgnoredDomains");

            if (!String.IsNullOrEmpty(ignoredDomains))
            {
                IgnoredDomains = ignoredDomains.Split(';').Select(d => d.Trim());
            }
        }

        public string SmtpServer => _provider.GetSetting<string>("appSettings", "AdInsure:Sogaz:Notification:Email:SmtpServer");

        public int? Port => _provider.GetSetting<int?>("appSettings", "AdInsure:Sogaz:Notification:Email:Port");

        public string SenderAddress => _provider.GetSetting<string>("appSettings", "AdInsure:Sogaz:Notification:Email:SenderAddress");

        public string Username => _provider.GetSetting<string>("appSettings", "AdInsure:Sogaz:Notification:Email:Username");

        public string Password => _provider.GetSetting<string>("appSettings", "AdInsure:Sogaz:Notification:Email:Password");

        public string Module => LocalizationServiceRegistrator.CORE;

        public IEnumerable<string> IgnoredDomains { get; }
    }
}
