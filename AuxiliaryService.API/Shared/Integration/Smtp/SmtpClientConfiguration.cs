using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Integration.Smtp
{
    public class SmtpClientConfiguration
    {
        /// <summary>
        /// host of smtp server
        /// </summary>
        public string ServerHost { get; set; }

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
    }
}
