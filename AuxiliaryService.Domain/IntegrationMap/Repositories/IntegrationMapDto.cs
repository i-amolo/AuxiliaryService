using NPoco;
using System;

namespace AuxiliaryService.Domain.IntegrationMap.Repositories
{
    [TableName("SYS_IMPL.INTEGRATION_MAP")]
    [PrimaryKey("MAP_ID", AutoIncrement = false)]
    public class IntegrationMapDto
    {
        [Column("MAP_ID")]
        public Guid MapId { get; private set; }

        [Column("PARENT_OBJECT_ID")]
        public string ParentObjectId { get; set; }

        [Column("ENTITY_TYPE_ID")]
        public string EntityTypeId { get; set; }

        [Column("OBJECT_ID")]
        public string ObjectID { get; set; }

        [Column("EXTERNAL_SYSTEM_ID")]
        public string ExternalSystemId { get; set; }

        [Column("EXTERNAL_OBJECT_ID")]
        public string ExternalObjectId { get; set; }

        [Column("UPDATE_TIME")]
        public DateTime UpdateTime { get; private set; }

        public IntegrationMapDto()
        {
            MapId = Guid.NewGuid();
            UpdateTime = DateTime.Now;
        }
    }
}