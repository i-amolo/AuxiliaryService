using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AuxiliaryService.ApplicationServices.Shared.Integration.Soap
{

    public class SoapMessageInspector : IClientMessageInspector
    {
        private readonly SoapMessageContainer _messageContainer;
        private bool _removeHeader;

        private void RemoveHeader(ref System.ServiceModel.Channels.Message request)
        {
            int headerIndexOfAction = request.Headers.FindHeader("Action", "http://schemas.microsoft.com/ws/2005/05/addressing/none");

            if (headerIndexOfAction >= 0)
            {
                request.Headers.RemoveAt(headerIndexOfAction);
            }
        }

        public SoapMessageInspector(SoapMessageContainer messageContainer, bool removeHeader = true)
        {
            _messageContainer = messageContainer;
            _removeHeader = removeHeader;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            if (_messageContainer != null)
            {
                _messageContainer.ResponseMessage = reply.ToString();
            }
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            if (_removeHeader)
            {
                // this removes SOAP header
                // to avoid MustUnderstand issue 
                RemoveHeader(ref request);
            }

            // log SOAP message if requested
            if (_messageContainer != null)
            {
                _messageContainer.RequestMessage = request.ToString();
            }

            return null;
        }
    }

    public class SoapEndpointBehavior : IEndpointBehavior
    {

        private readonly SoapMessageContainer _messageContainer;
        private bool _removeHeader;

        public SoapEndpointBehavior(SoapMessageContainer messageContainer, bool removeHeader = true)
        {
            _messageContainer = messageContainer;
            _removeHeader = removeHeader;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new SoapMessageInspector(_messageContainer, _removeHeader));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }

    public class SoapMessageContainer
    {
        public string RequestMessage { get; set; }
        public string ResponseMessage { get; set; }
    }

}
