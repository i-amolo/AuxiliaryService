using AuxiliaryService.API.Shared.StateMachine.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.StateMachine
{
    public interface IStateMachineConfiguration<TContext> 
        where TContext : StateMachineBaseContext
    {

        /// <summary>
        /// create a new configuration 
        /// </summary>
        /// <param name="smCode">state machine code</param>
        /// <param name="description">state machine description</param>
        /// <returns></returns>
        IStateMachineConfiguration<TContext> Create(string smCode, string description = null);

        /// <summary>
        /// add configured state
        /// </summary>
        /// <param name="transition"></param>
        /// <returns></returns>
        IStateConfiguration<TContext> Configure(StateMachineTransition transition);

        /// <summary>
        /// add configured state
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="description"></param>
        /// <param name="isInitial"></param>
        /// <returns></returns>
        IStateConfiguration<TContext> Configure(string from, string to, string description = null, bool isInitial = false);

        /// <summary>
        /// add configured state
        /// </summary>
        /// <param name="transition"></param>
        /// <returns></returns>
        IStateConfigurationAsync<TContext> ConfigureAsync(StateMachineTransition transition);

        /// <summary>
        /// add configured state
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="description"></param>
        /// <param name="isInitial"></param>
        /// <returns></returns>
        IStateConfigurationAsync<TContext> ConfigureAsync(string from, string to, string description = null, bool isInitial = false);

        /// <summary>
        /// enumerates list of configured states
        /// </summary>
        IEnumerable<IStateConfiguration<TContext>> States { get; }

        /// <summary>
        /// enumerates list of configured states
        /// </summary>
        IEnumerable<IStateConfigurationAsync<TContext>> StatesAsync { get; }

        /// <summary>
        /// creates and returns a new instance of SM
        /// it should be configured before
        /// </summary>
        IStateMachine<TContext> Instance(TContext context);

        /// <summary>
        /// creates and returns a new instance of SM
        /// it should be configured before
        /// </summary>
        IStateMachineAsync<TContext> InstanceAsync(TContext context);

    }
}
