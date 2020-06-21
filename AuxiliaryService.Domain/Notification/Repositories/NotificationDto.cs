using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.Notification.Repositories
{
    [TableName("SYS_IMPL.NOTIFICATION")]
    [PrimaryKey("NOTIFICATION_ID", AutoIncrement = false)]
    public class NotificationDto
    {
        [Column("NOTIFICATION_ID")]
        public Guid Id { get; set; }

        [Column("REF_ID")]
        public string RefId { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("PROVIDER_TYPE")]
        public string ProviderType { get; set; }

        [Column("PROVIDER_DETAILS")]  
        public string ProviderDetails { get; set; }

        [Column("NOTIFICATION_MESSAGE")]  
        public string Message { get; set; }

        [Column("SYS_CREATED_ON")]
        public DateTime SysCreatedOn{ get; set; }

        [Column("SYS_CREATED_BY_ID")]
        public Guid SysCreatedById{ get; set; }

    }
}
