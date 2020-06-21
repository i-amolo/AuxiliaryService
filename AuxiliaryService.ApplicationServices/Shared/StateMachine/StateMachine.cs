using Adacta.AdInsure.Framework.Core.Data.Transactions;
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
    public class StateMachine<TContext> : StateMachineBase<TContext>, IStateMachine<TContext> 
        where TContext : StateMachineBaseContext
    {

        public StateMachine(IStateMachineRepository repository,
                            IEnumerable<StateConfigurationBase<TContext>> states,
                            StateMachineData<TContext> data)
            :base(repository, states, data) {}

        public StateMachine(IStateMachineRepository repository,
                            IEnumerable<StateConfigurationBase<TContext>> states)
                            : base(repository, states, new StateMachineData<TContext>()) {}

        #region private fields

        #endregion

        #region IStateMachine

        public void Start()
        {
            InitBeforeStart();

            var prevRunInfo = _data.Progress.LastOrDefault();

            var runningState = prevRunInfo != null
                ? GetNextState(prevRunInfo.Transition)
                : GetInitialState();

            while (runningState != null)
            {
                var currentRunInfo = CreateRunInfo(prevRunInfo, runningState);

                RunTransition((StateConfiguration<TContext>)runningState, currentRunInfo);

                prevRunInfo = currentRunInfo;
                runningState = GetNextState(runningState.Transition);
            }

            SaveSuccessPoint(false);

        }
        
        public IStateMachine<TContext> Resume(Action<TContext> onResumeAction = null)
        {
            return base.ResumeInternal(onResumeAction) as IStateMachine<TContext>;
        }

        #endregion IStateMachine

        #region private methods

        private bool RunTransition(StateConfiguration<TContext> runningState, 
                                   StateMachineTransitionRunInfo currentRunInfo)
        {
            try
            {
                runningState.Action(Context);

                currentRunInfo.Status = StateMachineStatusConsts.Success;
                currentRunInfo.FinishDate = DateTime.Now;

                if (runningState.Transition.SavePoint)
                {
                    SaveSuccessPoint(true);
                }

                return true;
            }
            catch (Exception e)
            {
                currentRunInfo.Status = StateMachineStatusConsts.Fail;
                currentRunInfo.FinishDate = DateTime.Now;
                currentRunInfo.ErrorMessage = e.Message;
                SaveErrorPoint(e);
                throw;
            }
        }

        #endregion

    }
}
