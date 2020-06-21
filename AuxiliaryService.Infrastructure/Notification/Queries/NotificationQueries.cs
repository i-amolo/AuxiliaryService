using AuxiliaryService.Domain.Notification.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Infrastructure.Notification.Queries
{
    public class NotificationQueries : INotificationQueries
    {
        public string Select_Notifications()
        {
            return @"
  select NOTIFICATION_ID, 
	   REF_ID, 
	   PROVIDER_TYPE,
	   STATUS,
	   NOTIFICATION_MESSAGE,
	   PROVIDER_DETAILS,
	   SENT_ON,
	   SENT_DETAILS,
	   SYS_CREATED_ON,
	   SYS_CREATED_BY_ID
  from SYS_IMPL.NOTIFICATION
  where PROVIDER_TYPE = @providerType 
    and /**where**/";
        }


        public string Update_Sent()
        {
            return @"
UPDATE SYS_IMPL.NOTIFICATION SET
	STATUS = @Status,
	SENT_ON = @SentOn,
    SENT_DETAILS = @SentDetails
  WHERE NOTIFICATION_ID = @Id
";
        }

    }
}
