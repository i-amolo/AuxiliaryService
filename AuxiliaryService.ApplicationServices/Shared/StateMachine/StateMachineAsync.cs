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
    public class StateMachineAsync<TContext> : StateMachineBase<TContext>, IStateMachineAsync<TContext>
        where TContext : StateMachineBaseContext
    {

        public StateMachineAsync(IStateMachineRepository repository,
                            IEnumerable<StateConfigurationBase<TContext>> states,
                            StateMachineData<TContext> data)
            : base(repository, states, data) { }

        public StateMachineAsync(IStateMachineRepository repository,
                            IEnumerable<StateConfigurationBase<TContext>> states)
                            : base(repository, states, new StateMachineData<TContext>()) { }

        #region private fields

        #endregion

        #region IStateMachine

        public async Task StartAsync()
        {
            InitBeforeStart();

            var prevRunInfo = _data.Progress.LastOrDefault();

            var runningState = prevRunInfo != null
                ? GetNextState(prevRunInfo.Transition)
                : GetInitialState();

            while (runningState != null)
            {
                var currentRunInfo = CreateRunInfo(prevRunInfo, runningState);

                await RunTransition((StateConfigurationAsync<TContext>)runningState, currentRunInfo).ConfigureAwait(false);

                prevRunInfo = currentRunInfo;
                runningState = GetNextState(runningState.Transition);
            }

            SaveSuccessPoint(false);

        }

        public IStateMachineAsync<TContext> Resume(Action<TContext> onResumeAction = null)
        {
            return base.ResumeInternal(onResumeAction) as IStateMachineAsync<TContext>;
        }

        #endregion IStateMachine

        #region private methods

        private async Task RunTransition(StateConfigurationAsync<TContext> runningState,
                                         StateMachineTransitionRunInfo currentRunInfo)
        {
            try
            {
                await runningState.Action(Context);

                currentRunInfo.Status = StateMachineStatusConsts.Success;
                currentRunInfo.FinishDate = DateTime.Now;

                if (runningState.Transition.SavePoint)
                {
                    SaveSuccessPoint(true);
                }

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
