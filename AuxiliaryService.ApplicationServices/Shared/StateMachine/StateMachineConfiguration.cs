using AuxiliaryService.API.AdContract;
using AuxiliaryService.API.Shared.StateMachine;
using AuxiliaryService.API.Shared.StateMachine.DTO;
using AuxiliaryService.Domain.StateMachine.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.StateMachine
{
    public class StateMachineConfiguration<TContext> : IStateMachineConfiguration<TContext>
        where TContext : StateMachineBaseContext
    {

        private readonly IStateMachineRepository _repository;

        public StateMachineConfiguration(IStateMachineRepository repository)
        {
            _repository = repository;
        }

        private StateMachineData<TContext> _data;

        private List<StateConfigurationBase<TContext>> _states;

        public IEnumerable<IStateConfiguration<TContext>> States => _states.Cast<IStateConfiguration<TContext>>();
        public IEnumerable<IStateConfigurationAsync<TContext>> StatesAsync => _states.Cast<IStateConfigurationAsync<TContext>>();

        public IStateConfiguration<TContext> Configure(StateMachineTransition transition)
        {
            ValidateInitialized();

            var stateCfg = new StateConfiguration<TContext>(transition);
            _states.Add(stateCfg);
            return stateCfg;
        }

        public IStateConfiguration<TContext> Configure(string from, string to, string description = null, bool isInitial = false)
        {
            var trans = new StateMachineTransition(from, to, description, isInitial);
            return Configure(trans);
        }

        public IStateConfigurationAsync<TContext> ConfigureAsync(StateMachineTransition transition)
        {
            ValidateInitialized();

            var stateCfg = new StateConfigurationAsync<TContext>(transition);
            _states.Add(stateCfg);
            return stateCfg;
        }

        public IStateConfigurationAsync<TContext> ConfigureAsync(string from, string to, string description = null, bool isInitial = false)
        {
            var trans = new StateMachineTransition(from, to, description, isInitial);
            return ConfigureAsync(trans);
        }

        public IStateMachineConfiguration<TContext> Create(string smCode, string description = null)
        {
            AdContract.NotNullNorEmpty(smCode);

            Init();

            _data.Code = smCode;
            _data.Description = description;

            return this;
        }

        public IStateMachine<TContext> Instance(TContext context)
        {
            AdContract.NotNull(context);
            AdContract.Requires(context.RefId != Guid.Empty);

            ValidateConfigured();

            _data.Context = context;
            _data.InstanceId = Guid.NewGuid();
            _data.RefId = context.RefId;
            var sm = new StateMachine<TContext>(_repository, _states, _data);

            Release();

            return sm;
        }

        public IStateMachineAsync<TContext> InstanceAsync(TContext context)
        {
            AdContract.NotNull(context);
            AdContract.Requires(context.RefId != Guid.Empty);

            ValidateConfigured();

            _data.Context = context;
            _data.InstanceId = Guid.NewGuid();
            _data.RefId = context.RefId;
            var sm = new StateMachineAsync<TContext>(_repository, _states, _data);

            Release();

            return sm;
        }

        #region private methods

        private void ValidateInitialized()
        {
            if (_data == null || string.IsNullOrEmpty(_data.Code) || _states == null )
                throw new Exception("You are trying to use an uninitialized state machine. Please invoke Create() first");
        }

        private void ValidateConfigured()
        {
            ValidateInitialized();

            if (_states.Count == 0)
                throw new Exception("You are trying to create an instance of a not configured state machine. Please put proper states configuration");

            if(_states.Count(a => a.Transition.IsInitial) != 1)
                throw new Exception("There should be one and only one initial transision");

            ValidateTraverse();

        }

        private void ValidateTraverse()
        {
            var initial = _states.Single(a => a.Transition.IsInitial);
            var traversedStates = new HashSet<StateConfigurationBase<TContext>>();

            var current = initial;

            while (current != null)
            {
                var to = _states.Where(a => a.Transition.From == current.Transition.To);

                if (to.Count() > 1)
                    throw new Exception($"More then one target states found for the state {current.Transition.To}");

                current = to.SingleOrDefault();

                if (current != null)
                {
                    if (traversedStates.Any(a => ReferenceEquals(a, current)))
                        throw new Exception($"Cycled path found on state {current.Transition.To}");

                    traversedStates.Add(current);
                }

            }
        }

        private void Release()
        {
            _data = null;
            _states = null;
        }

        private void Init()
        {
            _data = new StateMachineData<TContext>();
            _states = new List<StateConfigurationBase<TContext>>();
        }


        #endregion

    }
}
