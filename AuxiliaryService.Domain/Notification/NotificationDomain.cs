using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification
{
    public class NotificationDomain<TProviderDetails> where TProviderDetails: NotificationProviderDetailsDomainBase
    {
        /// <summary>
        /// notification id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// notification reference id
        /// </summary>
        public string RefId { get; set; }

        /// <summary>
        /// notification status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// notification provider type
        /// </summary>
        public string ProviderType { get; set; }

        /// <summary>
        /// provider details 
        /// </summary>
        public TProviderDetails ProviderDetails { get; set; }

        /// <summary>
        /// message
        /// </summary>
        public NotificationMessageDomain Message { get; set; }

        /// <summary>
        /// date of sending
        /// </summary>
        public DateTime? SentOn { get; set; }

    }
}
