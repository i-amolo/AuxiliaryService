using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Queries
{
    public interface INotificationQueries
    {

        /// <summary>
        /// SQL to retrieve notifications
        /// </summary>
        /// <returns></returns>
        string Select_Notifications();

        /// <summary>
        /// update notification when it has been sent
        /// </summary>
        /// <returns></returns>
        string Update_Sent();
    }
}
