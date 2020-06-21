using AuxiliaryService.API.Shared.Integration.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.Integration.Smtp
{
    public class SmtpClient : ISmtpClient
    {

        #region private methods

        private void ValidateClient(SmtpClientConfiguration configuration)
        {
            if (string.IsNullOrEmpty(configuration.ServerHost))
                throw new Exception("SMTP client configuration failed. Server isn't specified.");

            if (configuration.Port == default(int))
                throw new Exception("SMTP client configuration failed. Port isn't specified.");

            if (string.IsNullOrEmpty(configuration.UserName))
                throw new Exception("SMTP client configuration failed. Username isn't specified.");

            if (string.IsNullOrEmpty(configuration.Password))
                throw new Exception("SMTP client configuration failed. Password isn't specified.");

        }

        private void ValidateRequest(SmtpRequest request)
        {
            if (request.Recipients == null || !request.Recipients.Any())
                throw new Exception("SMTP request failed. At least one recipient must be specified.");

            if (request.Recipients.Any(a => string.IsNullOrEmpty(a)))
                throw new Exception("Empty address found");

        }

        private System.Net.Mail.SmtpClient CreateClient(SmtpClientConfiguration configuration)
        {
            ValidateClient(configuration);

            var client = new System.Net.Mail.SmtpClient();
            client.Port = configuration.Port;
            client.Host = configuration.ServerHost;

            // SSL temporarily disabled because platform 3.1.3 only supports TLS 1.1 and 1.2 but SMTP server does not
            //client.EnableSsl = true;

            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(configuration.UserName, configuration.Password);

            return client;
        }

        private void SendInternal(System.Net.Mail.SmtpClient client, SmtpRequest request)
        {

            ValidateRequest(request);

            var message = new MailMessage();
            message.BodyEncoding = request.MessageEncoding ?? Encoding.UTF8;
            message.SubjectEncoding = request.MessageEncoding ?? Encoding.UTF8;

            message.From = new MailAddress(request.Sender);

            if (request.CC != null && request.CC.Any())
            {
                request.CC.ForEach(addr => message.CC.Add(addr)); 
            }

            request.Recipients.ForEach(addr => message.To.Add(addr));

            message.Subject = request.Subject;
            message.Body = request.Body;
            message.IsBodyHtml = request.IsBodyHtml;

            message.DeliveryNotificationOptions = DeliveryNotificationOptions.None;

            if (request.Attachments != null)
            {
                foreach (var attachment in request.Attachments)
                {
                    message.Attachments.Add(new Attachment(new System.IO.MemoryStream(attachment.Data), attachment.Name, attachment.MediaType));
                }
            }

            client.Send(message);
        }

        #endregion 
        
        #region ISmtpClient

        public void Send(SmtpClientConfiguration configuration, SmtpRequest request)
        {
            var client = CreateClient(configuration);
            SendInternal(client, request);
        }

        #endregion 
    }
}
