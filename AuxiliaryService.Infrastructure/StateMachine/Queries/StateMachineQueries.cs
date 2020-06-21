using AuxiliaryService.Domain.StateMachine.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Infrastructure.StateMachine.Queries
{
    public class StateMachineQueries : IStateMachineQueries
    {
        public string Select_GetStateMachineByCodeRef()
        {
            return @"select * from sys_impl.state_machine where code = @code and ref_id = @refId";
        }

        public string Select_GetStateMachineByInstanceId()
        {
            return @"select * from sys_impl.state_machine where instance_id = @instanceId";
        }
    }
}
