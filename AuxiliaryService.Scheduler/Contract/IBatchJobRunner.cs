using AuxiliaryService.Scheduler.Contract;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.API
{
    public interface IBatchJobRunner
    {

        /// <summary>
        /// register a batch jobs to be scheduled and run
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        void RegisterBatchJobs(IEnumerable<IBatchJobConfiguration> cfgs);

        /// <summary>
        /// start scheduler
        /// </summary>
        /// <returns></returns>
        void Start();
    }
}
