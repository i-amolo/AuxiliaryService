using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Settings
{

    public class SystemSettingsFactory : ISystemSettingsFactory
    {
        /// <summary>
        /// Create PAS integration settings class with specified provider
        /// </summary>
        /// <param name="provider">Config configuration settings provider</param>
        /// <returns>PAS integration settings</returns>
        public SystemSettings Create(IConfigConfigurationSettingsProvider provider)
        {
            return new SystemSettings(provider);
        }
    }
}
