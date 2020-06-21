using Common.Logging;
using Ninject;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Infrastructure
{
    class NinjectJobFactory : IJobFactory
    {
        readonly IKernel provider;
        private readonly Lazy<ILog> _logger = new Lazy<ILog>(() => LogManager.GetLogger("Scheduler"));

        public NinjectJobFactory(IKernel provider)
        {
            this.provider = provider;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;
            try
            {
                // this will inject any dependencies that the job requires
                var value = provider.GetService(jobType) as IJob;
                return value;
            }
            catch (Exception e)
            {
                _logger.Value.Error($"Job creation failed. Job key: {bundle.JobDetail.Key}. Error: {e}");
                throw new SchedulerException($"Job creation failed. Job key: {bundle.JobDetail.Key}", e);
            }
        }

        public void ReturnJob(IJob job)
        {
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
