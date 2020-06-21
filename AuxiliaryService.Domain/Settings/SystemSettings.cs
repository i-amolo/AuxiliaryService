using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Settings
{
    public class SystemSettings : ISystemSettings
    {
        private readonly IConfigConfigurationSettingsProvider _provider;

        private const string AppSettingsSection = "appSettings";

        public SystemSettings(IConfigConfigurationSettingsProvider provider)
        {
            _provider = provider;
        }

        public string Module => LocalizationServiceRegistrator.CORE;

        public string ClientRootUrl => _provider.GetSetting<string>(AppSettingsSection, "ClientRootUrl");
    }
}
