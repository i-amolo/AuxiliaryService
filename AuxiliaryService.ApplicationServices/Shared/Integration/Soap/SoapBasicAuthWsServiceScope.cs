using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.Integration.Soap
{

    public class SoapBasicAuthWsServiceScope
    {
       
        private readonly OperationContextScope m_scope;

        public SoapBasicAuthWsServiceScope(IClientChannel channel)
        {
            m_scope = new OperationContextScope(channel);
        }

        public IDisposable ClientChannel(string username, string password)
        {
            var basicHttpAuth = SoapBasicHttpAuth(username, password);
            return new ClientChannelItem(m_scope, basicHttpAuth);
        }

        private class ClientChannelItem : IDisposable
        {
            private readonly OperationContextScope m_scope;

            public ClientChannelItem(OperationContextScope scope, string basicHttpAuth)
            {
                m_scope = scope;

                var httpRequestProperty = new HttpRequestMessageProperty();

                httpRequestProperty.Headers[HttpRequestHeader.Authorization] = basicHttpAuth;

                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
            }

            public void Dispose()
            {
                m_scope.Dispose();
            }
        }
        
        private string SoapBasicHttpAuth(string username, string password)
        {
            var utf8 = new UTF8Encoding().GetBytes($"{username}:{password}");
            var basicHttpAuth = string.Format("Basic {0}", Convert.ToBase64String(utf8));
            return basicHttpAuth;
        }

    }
}
