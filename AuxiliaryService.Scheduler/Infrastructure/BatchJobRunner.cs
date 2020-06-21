using AuxiliaryService.API.ContextInitializer;
using AuxiliaryService.Scheduler.API;
using AuxiliaryService.Scheduler.Contract;
using Common.Logging;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Infrastructure
{

    public class BatchJobRunner : IBatchJobRunner 
    {
        #region private fields

        private readonly IScheduler _scheduler;
        private readonly Lazy<ILog> _logger = new Lazy<ILog>(() => LogManager.GetLogger("Scheduler"));
        private readonly IApplicationContextInitializerFactory _applicationContextInitializerFactory;

        #endregion

        #region ctor

        public BatchJobRunner(IScheduler scheduler, 
                              IApplicationContextInitializerFactory applicationContextInitializerFactory)
        {
            _scheduler = scheduler;
            _applicationContextInitializerFactory = applicationContextInitializerFactory;
        }

        #endregion   

        #region IBatchJobRunner

        public void RegisterBatchJobs(IEnumerable<IBatchJobConfiguration> cfgs)
        {
            try
            {
                _logger.Value.Trace($"Registration jobs. Count: {cfgs.Count()}");

                var dict = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();

                foreach (var cfg in cfgs)
                {
                    // create job
                    var job = cfg.CreateJobDetail();

                    // set initalizer and pass it as data map into the job
                    var initializer = _applicationContextInitializerFactory.CreateInitializer(cfg.UserId, cfg.ActorCode);
                    job.JobDataMap.Put("ContextInitializer", initializer);

                    // create trigger
                    var triggers = new List<ITrigger> { cfg.CreateTrigger() };
                    dict.Add(job, new ReadOnlyCollection<ITrigger>(triggers));
                }

                var readOnlyDict = new ReadOnlyDictionary<IJobDetail, IReadOnlyCollection<ITrigger>>(dict);

                _scheduler.ScheduleJobs(readOnlyDict, true).Wait();

                _logger.Value.Trace($"Registration succeeded.");

            }
            catch (Exception e)
            {
                _logger.Value.Error($"Registration failed. Error: {e.ToString()}");
            }

        }

        public void Start()
        {
            _logger.Value.Trace("Scheduler starting.");

            try
            {
                _scheduler.Start().Wait();
                 
                _logger.Value.Trace("Scheduler started successfully.");

            }
            catch (Exception e)
            {
                _logger.Value.Error($"Scheduler starting failed. Error: {e.ToString()}");
            }

        }

        #endregion 
    }
}
