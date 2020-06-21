using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.Integration.Smtp
{
    public interface ISmtpClient
    {
        /// <summary>
        /// send smtp request
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        void Send(SmtpClientConfiguration configuration, SmtpRequest request);
    }
}
