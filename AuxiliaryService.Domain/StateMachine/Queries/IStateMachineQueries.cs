using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.StateMachine.Queries
{
    public interface IStateMachineQueries
    {
        /// <summary>
        /// SQL query to retrieve SM by code and ref_id
        /// </summary>
        /// <returns></returns>
        string Select_GetStateMachineByCodeRef();

        /// <summary>
        /// SQL query to retrieve SM instance_id
        /// </summary>
        /// <returns></returns>
        string Select_GetStateMachineByInstanceId();
    }
}
