using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Base
{
    public abstract class JobWorkerBase : IJob
    {
        #region private fields

        private readonly Lazy<ILog> _logger = new Lazy<ILog>(() => LogManager.GetLogger("Scheduler"));

        #endregion

        #region protected

        protected abstract void ExecuteAction(IJobExecutionContext context);

        #endregion

        #region private methods

        private void ExecuteActionInternal(IJobExecutionContext context)
        {
            // initialize context
            // it's important to do it within calling thread
            var initializer = context.JobDetail.JobDataMap.Get("ContextInitializer") as IConsumerApplicationContextInitializer;
            initializer.Initialize();

            // do work
            ExecuteAction(context);
        }

        #endregion

        #region IJob

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Value.Trace($"Job key: {context.JobDetail.Key}. Started.");

                await Task.Run(() => ExecuteActionInternal(context));

                _logger.Value.Trace($"Job key: {context.JobDetail.Key}. Finished.");

            }
            catch (Exception e)
            {
                _logger.Value.Error($"Job key: {context.JobDetail.Key}. Failed. Error: {e.ToString()}");
            }

        }

        #endregion

    }
}
