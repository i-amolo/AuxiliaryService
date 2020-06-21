using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Services
{
    public interface INotificationProviderResolver
    {
        /// <summary>
        /// resolve provider by provider type
        /// </summary>
        /// <returns></returns>
        INotificationProvider<TProviderDetails> Resolve<TProviderDetails>(string providerType) where TProviderDetails : NotificationProviderDetailsDomainBase;

        /// <summary>
        /// register ontification provider
        /// </summary>
        /// <typeparam name="TProvider"></typeparam>
        /// <param name="providerType"></param>
        void Register(INotificationProvider provider, string providerType);
    }
}
