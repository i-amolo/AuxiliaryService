using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuxiliaryService.Scheduler.Contract
{

    public interface IBatchJobConfiguration
    {
        /// <summary>
        /// specify trigger 
        /// </summary>
        /// <returns></returns>
        ITrigger CreateTrigger();

        /// <summary>
        /// specify job details
        /// </summary>
        /// <returns></returns>
        IJobDetail CreateJobDetail(); 

        /// <summary>
        /// specify job type
        /// </summary>
        Type GetJobType { get; }

        /// <summary>
        /// specify user Id under which job is being run
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// specify actor code under which job is being run
        /// </summary>
        string ActorCode { get; }
    }

}
