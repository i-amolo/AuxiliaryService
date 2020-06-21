using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.StateMachine
{
    [TableName("SYS_IMPL.STATE_MACHINE")]
    [PrimaryKey("STATE_MACHINE_ID", AutoIncrement = false)]
    public class StateMachineDto
    {
        [Column("STATE_MACHINE_ID")]
        public Guid StateMachineId { get; set; }

        [Column("INSTANCE_ID")]
        public Guid InstanceId { get; set; }

        [Column("CODE")]
        public string Code { get; set; }

        [Column("REF_ID")]
        public Guid RefId { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("STATUS")]
        public string Status { get; set; }

        [Column("PROGRESS")]
        public string Progress { get; set; }

        [Column("CONTEXT")]
        public string Context { get; set; }

        [Column("ERROR")]
        public string Error { get; set; }

    }
}
