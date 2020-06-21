using Adacta.AdInsure.Framework.Core.Data.Transactions;
using Adacta.AdInsure.Framework.Core.Exceptions;
using AuxiliaryService.API.AdContract;
using AuxiliaryService.API.Shared.StateMachine;
using AuxiliaryService.API.Shared.StateMachine.Consts;
using AuxiliaryService.API.Shared.StateMachine.DTO;
using AuxiliaryService.Domain.StateMachine.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.ApplicationServices.Shared.StateMachine
{
    public abstract class StateMachineBase<TContext> : IStateMachineBase<TContext>
        where TContext : StateMachineBaseContext
    {

        #region protected fields

        protected readonly IStateMachineRepository _repository;
        protected StateMachineData<TContext> _data;
        
        #endregion 

        public List<StateMachineTransitionRunInfo> Progress
        {
            get { return _data.Progress; }
            set { _data.Progress = value; }
        } 

        public IEnumerable<StateConfigurationBase<TContext>> States { get; set; }

        public string SmCode => _data.Code;
        public Guid InstanceId => _data.InstanceId;
        public TContext Context => (TContext)_data.Context;

        public Guid RunId { get; private set; }

        public StateMachineBase(IStateMachineRepository repository,
                                IEnumerable<StateConfigurationBase<TContext>> states,
                                StateMachineData<TContext> data)
        {
            _repository = repository;
            _data = data;
            States = states;
        }

        public StateMachineBase(IStateMachineRepository repository,
                                IEnumerable<StateConfigurationBase<TContext>> states)
                                : this(repository, states, new StateMachineData<TContext>()) {}

        #region private fields

        #endregion

        #region IStateMachine
        
        protected virtual IStateMachineBase<TContext> ResumeInternal(Action<TContext> onResumeAction = null)
        {
            AdContract.NotNull(_data);
            AdContract.NotNull(_data.Code);
            AdContract.NotNull(_data.Context);
            AdContract.Requires(_data.Context.RefId != Guid.Empty);

            var restoredData = StateMachineConverter.Convert<TContext>(_repository.Get(_data.Code, _data.Context.RefId));

            // assign data only if restored data found
            if (restoredData != null)
            {
                _data = restoredData;

                _data.Error = null;

                RestoreExecutionStack();
            }

            _data.Status = _data.Status != StateMachineStatusConsts.Success
                ? StateMachineStatusConsts.InProgress
                : throw new BusinessException("Can not resume state machine because it was already successfully passed");

            onResumeAction?.Invoke((TContext)_data.Context);

            return this;
        }

        #endregion IStateMachine

        #region protected methods

        protected void RestoreExecutionStack()
        {
            // we start with the next step after the last successful save point
            // otherwise, go to initial
            
            // remove actions until either successful save point or initial
            var passedSavePoints = _data
                .Progress
                .Where(a => a.Transition.SavePoint && a.Status == StateMachineStatusConsts.Success);

            if (passedSavePoints.Any())
            {
                var maxScsOrderNum = passedSavePoints.Max(a => a.OrderNum);
                var stepToRemove = Progress.Where(a => a.OrderNum > maxScsOrderNum);
                stepToRemove.ToList().ForEach(a => _data.Progress.Remove(a));
            }
            else
            {
                _data.Progress.Clear();
            }

        }

        protected void InitBeforeStart()
        {
            Progress = Progress ?? new List<StateMachineTransitionRunInfo>();
            RunId = Guid.NewGuid();
        }
        
        protected StateMachineTransitionRunInfo CreateRunInfo(StateMachineTransitionRunInfo prevRunInfo, 
                                                              StateConfigurationBase<TContext> runningState)
        {
            var currentRunInfo = new StateMachineTransitionRunInfo()
            {
                Transition = runningState.Transition,
                RunId = this.RunId,
                OrderNum = (prevRunInfo?.OrderNum).GetValueOrDefault() + 1, 
                StartDate = DateTime.Now,
                Status = StateMachineStatusConsts.InProgress
            };
            Progress.Add(currentRunInfo);
            return currentRunInfo;
        }

        protected StateConfigurationBase<TContext> GetInitialState()
        {
            return States.SingleOrDefault(a => a.Transition.IsInitial);
        }

        protected StateConfigurationBase<TContext> GetNextState(StateMachineTransition current)
        {
            return States.Where(a => a.Transition.From == current.To).SingleOrDefault();
        }

        protected void SaveSuccessPoint(bool intermidiatePoint)
        {
            _data.Status = intermidiatePoint ? StateMachineStatusConsts.InProgress : StateMachineStatusConsts.Success;

            var domainDto = StateMachineConverter.Convert(_data);
            _repository.Save(domainDto);
            _data.StateMachineId = domainDto.StateMachineId;
        }

        protected void SaveErrorPoint(Exception e)
        {
            _data.Status = StateMachineStatusConsts.Fail;
            _data.Error = e.ToString();

            var domainDto = StateMachineConverter.Convert(_data);
            _repository.Save(domainDto);
            _data.StateMachineId = domainDto.StateMachineId;

            // log error
        }

        #endregion

    }
}
