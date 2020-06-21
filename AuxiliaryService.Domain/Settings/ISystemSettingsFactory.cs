using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Settings
{
    public interface ISystemSettingsFactory : IConfigurationSettingFactory<SystemSettings, IConfigConfigurationSettingsProvider>
    {
    }
}
