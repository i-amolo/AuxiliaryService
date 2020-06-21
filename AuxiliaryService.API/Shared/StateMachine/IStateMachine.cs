using AuxiliaryService.API.Shared.StateMachine.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.StateMachine
{

    public interface IStateMachineBase<TContext>
        where TContext : StateMachineBaseContext
    {
        /// <summary>
        /// State machine code
        /// </summary>
        string SmCode { get; }

        /// <summary>
        /// state machine instance is just like an object of class (configuration)
        /// </summary>
        Guid InstanceId { get; }

        /// <summary>
        /// Statem machine Run Id
        /// one state machine instance can be run multiple times
        /// so Run Id is unique for one particular run of SM instance
        /// </summary>
        Guid RunId { get; }

        /// <summary>
        /// context object SM works with
        /// </summary>
        TContext Context { get; }
    }

    public interface IStateMachine<TContext> : IStateMachineBase<TContext>
        where TContext : StateMachineBaseContext
    {
        
        /// <summary>
        /// start SM 
        /// if Resume is run, then Start with resumed step 
        /// </summary>
        void Start();

        /// <summary>
        /// Resumes SM instance to go forward
        /// if the last state fails, it restores context and tries to run it again
        /// if the last state succeded, it restores context and runs from the next state
        /// if there is no stored context, it runs Start()
        /// </summary>
        /// <param name="onResumeAction">it can be used to enrich context with cutsom non restorable data</param>
        IStateMachine<TContext> Resume(Action<TContext> onResumeAction = null);

    }

    public interface IStateMachineAsync<TContext> : IStateMachineBase<TContext>
        where TContext : StateMachineBaseContext
    {
        
        /// <summary>
        /// start SM as async operation
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Resumes SM instance to go forward
        /// if the last state fails, it restores context and tries to run it again
        /// if the last state succeded, it restores context and runs from the next state
        /// if there is no stored context, it runs Start()
        /// </summary>
        /// <param name="onResumeAction">it can be used to enrich context with cutsom non restorable data</param>
        IStateMachineAsync<TContext> Resume(Action<TContext> onResumeAction = null);

    }
}
