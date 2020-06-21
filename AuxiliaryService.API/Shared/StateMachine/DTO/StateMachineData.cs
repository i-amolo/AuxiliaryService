using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.StateMachine.DTO
{
    public class StateMachineData<TContext> where TContext : StateMachineBaseContext
    {
        public Guid StateMachineId { get; set; }
        public string Code { get; set; }
        public Guid RefId { get; set; }
        public Guid InstanceId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Error { get; set; }
        public List<StateMachineTransitionRunInfo> Progress { get; set; }
        public TContext Context { get; set; }
    }
}
