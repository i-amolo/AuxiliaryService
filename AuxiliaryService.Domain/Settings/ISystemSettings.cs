using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Settings
{

    public interface ISystemSettings : IConfigConfigurationSettings
    {
        /// <summary>
        /// root part of client application URL like https:\\localhost:60003\#
        /// </summary>
        string ClientRootUrl { get; }

    }

}
