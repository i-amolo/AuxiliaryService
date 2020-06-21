using AuxiliaryService.API.Shared.StateMachine;
using AuxiliaryService.API.Shared.StateMachine.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.StateMachine
{

    public class StateConfigurationBase<TContext> where TContext : StateMachineBaseContext
    {
        public StateMachineTransition Transition { get; protected set; }
    }

    public class StateConfiguration<TContext> : StateConfigurationBase<TContext>, IStateConfiguration<TContext> 
        where TContext : StateMachineBaseContext
    {

        public StateConfiguration(StateMachineTransition transition)
        {
            Transition = transition;
        }
        
        public Action<TContext> Action { get; private set; }

        public IStateConfiguration<TContext> Do(Action<TContext> action)
        {
            Action = action;
            return this;
        }

        public IStateConfiguration<TContext> OnException(Action<TContext> action)
        {
            throw new NotImplementedException();
        }

        public IStateConfiguration<TContext> SavePoint()
        {
            Transition.SavePoint = true;
            return this;
        }
    }

    public class StateConfigurationAsync<TContext> : StateConfigurationBase<TContext>, IStateConfigurationAsync<TContext> 
        where TContext : StateMachineBaseContext
    {

        public StateConfigurationAsync(StateMachineTransition transition)
        {
            Transition = transition;
        }

        public Func<TContext, Task> Action { get; private set; }

        public IStateConfigurationAsync<TContext> Do(Func<TContext, Task> action)
        {
            Action = action;
            return this;
        }

        public IStateConfigurationAsync<TContext> OnException(Action<TContext> action)
        {
            throw new NotImplementedException();
        }

        public IStateConfigurationAsync<TContext> SavePoint()
        {
            Transition.SavePoint = true;
            return this;
        }

    }

}
