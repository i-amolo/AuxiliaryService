using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Repositories
{
    public interface INotificationRepository
    {
        /// <summary>
        /// persists notification
        /// </summary>
        /// <typeparam name="TProviderDetails"></typeparam>
        /// <param name="notification"></param>
        /// <returns></returns>
        NotificationDomain<TProviderDetails> Save<TProviderDetails>(NotificationDomain<TProviderDetails> notification)
            where TProviderDetails : NotificationProviderDetailsDomainBase;

        /// <summary>
        /// retrieves notifications by ref Id
        /// </summary>
        /// <typeparam name="TProviderDetails"></typeparam>
        /// <param name="refId"></param>
        /// <param name="providerType"></param>
        /// <returns></returns>
        IEnumerable<NotificationDomain<TProviderDetails>> Get<TProviderDetails>(List<Guid> ids, List<string> refIds, string providerType)
            where TProviderDetails : NotificationProviderDetailsDomainBase;

        /// <summary>
        /// set status when sent OK
        /// </summary>
        /// <typeparam name="TProviderDetails"></typeparam>
        /// <param name="notification"></param>
        /// <returns></returns>
        void SetNotificationSent<TProviderDetails>(NotificationDomain<TProviderDetails> notification)
            where TProviderDetails : NotificationProviderDetailsDomainBase;

        /// <summary>
        /// set status when error
        /// </summary>
        /// <typeparam name="TProviderDetails"></typeparam>
        /// <param name="notification"></param>
        void SetNotificationError<TProviderDetails>(NotificationDomain<TProviderDetails> notification, Exception e)
            where TProviderDetails : NotificationProviderDetailsDomainBase;

    }
}
