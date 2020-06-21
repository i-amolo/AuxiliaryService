using Adacta.AdInsure.Framework.Core.API.Shared.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.DomainEvent
{
    public class NotificationDomainEvent : IDomainEvent
    {
        public Guid NotificationId { get; private set; }
        public string RefId { get; private set; }
        public string ProviderType { get; private set; }

        public NotificationDomainEvent(Guid notificationId, string refId, string providerType)
        {
            NotificationId = notificationId;
            RefId = refId;
            ProviderType = providerType;
        }
    }
}
