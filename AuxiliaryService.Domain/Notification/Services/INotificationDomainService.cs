using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Services
{
    public interface INotificationDomainService
    {
        /// <summary>
        /// get notifications by ref Id
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<NotificationDomain<TProviderDetails>> GetByRef<TProviderDetails>(string refId, string providerType)
            where TProviderDetails : NotificationProviderDetailsDomainBase;

        /// <summary>
        /// create notification
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        NotificationDomain<TProviderDetails> Create<TProviderDetails>(NotificationDomain<TProviderDetails> notification)
            where TProviderDetails : NotificationProviderDetailsDomainBase;

        /// <summary>
        /// execute notifications by criteria
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<NotificationResponseDomain> Notify<TProviderDetails>(NotificationCriteriaDomain criteria)
            where TProviderDetails : NotificationProviderDetailsDomainBase;
    }
}
