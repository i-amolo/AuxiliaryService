using Adacta.AdInsure.Framework.Core.API.Shared.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Messages
{

    public class NotificationDomainEventMsg : IMessageBody
    {
        public Guid NotificationId { get; private set; }
        public string RefId { get; private set; }
        public string ProviderType { get; private set; }


        public NotificationDomainEventMsg(Guid notificationId, string refId, string providerType)
        {
            NotificationId = notificationId;
            RefId = refId;
            ProviderType = providerType;
        }

        public string GetMessageKey()
        {
            return Guid.NewGuid().ToString();
        }
    }

}
