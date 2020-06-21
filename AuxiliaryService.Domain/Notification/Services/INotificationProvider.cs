using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Services
{

    public interface INotificationProvider { }

    public interface INotificationProvider<TProviderDetails> : INotificationProvider
        where TProviderDetails : NotificationProviderDetailsDomainBase
    {
        /// <summary>
        /// execute notifications
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        void Notify(NotificationDomain<TProviderDetails> request);

        /// <summary>
        /// set default values
        /// </summary>
        /// <param name="request"></param>
        void SetDefault(NotificationDomain<TProviderDetails> request);
        
        /// <summary>
        /// validate
        /// </summary>
        /// <param name="request"></param>
        void Validate(NotificationDomain<TProviderDetails> request);
    }
}
