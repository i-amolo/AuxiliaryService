using AuxiliaryService.API.ContextInitializer;
using AuxiliaryService.Scheduler.Contract;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Base
{
    public abstract class ScheduleBatchJobConfigurationBase<TJob> : IBatchJobConfiguration
        where TJob: IJob
    {

        #region protected 

        /// <summary>
        /// implement triggering logic 
        /// </summary>
        /// <param name="schedulerBuilder"></param>
        protected abstract ITrigger CreateTriggerInternal();

        /// <summary>
        /// implement job details
        /// </summary>
        /// <returns></returns>
        protected abstract IJobDetail CreateJobInternal();

        /// <summary>
        /// specify job identity
        /// </summary>
        /// <returns></returns>
        protected virtual string JobIdentity => Guid.NewGuid().ToString(); 

        /// <summary>
        /// specify group identity
        /// </summary>
        /// <returns></returns>
        protected virtual string JobGroup => "Sample"; 

        /// <summary>
        /// set job addittional params
        /// </summary>
        /// <param name="jobParams"></param>
        protected virtual void SetJobParams(JobDataMap jobParams) { }

        #endregion

        #region private

        #endregion 

        #region IBatchJobConfiguration

        public ITrigger CreateTrigger()
        {
            return CreateTriggerInternal();
        }

        public IJobDetail CreateJobDetail()
        {
            return CreateJobInternal();
        }

        public Type GetJobType => typeof(TJob);

        public abstract Guid UserId { get; }

        public abstract string ActorCode { get; }

        #endregion


    }
}
