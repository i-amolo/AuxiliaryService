using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Domain.StateMachine.Repositories
{
    public interface IStateMachineRepository
    {
        /// <summary>
        /// save state machine
        /// </summary>
        /// <param name="dto"></param>
        void Save(StateMachineDto dto);

        /// <summary>
        /// get by instance Id
        /// </summary>
        /// <param name="instanceId"></param>
        /// <returns></returns>
        StateMachineDto Get(Guid instanceId);

        /// <summary>
        /// get by SM code & refId
        /// </summary>
        /// <param name="code"></param>
        /// <param name="refId"></param>
        /// <returns></returns>
        StateMachineDto Get(string code, Guid refId);


    }
}
