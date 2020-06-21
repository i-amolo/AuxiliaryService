using AuxiliaryService.API.Shared.StateMachine.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.API.Shared.StateMachine
{

    public interface IStateConfiguration<TContext> where TContext: StateMachineBaseContext
    {
        IStateConfiguration<TContext> Do(Action<TContext> action);
        IStateConfiguration<TContext> SavePoint();
        IStateConfiguration<TContext> OnException(Action<TContext> action);
    }

    public interface IStateConfigurationAsync<TContext> where TContext: StateMachineBaseContext
    {
        IStateConfigurationAsync<TContext> Do(Func<TContext, Task> action);
        IStateConfigurationAsync<TContext> SavePoint();
        IStateConfigurationAsync<TContext> OnException(Action<TContext> action);
    }



}
