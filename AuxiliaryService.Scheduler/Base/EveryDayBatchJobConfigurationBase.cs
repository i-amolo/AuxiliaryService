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
    public abstract class EveryDayBatchJobConfigurationBase<TJob> : ScheduleBatchJobConfigurationBase<TJob>
        where TJob: IJob
    {

        #region protected

        /// <summary>
        /// derived class specifies the Hour (0-23) of the day when a trigger is fired
        /// </summary>
        protected abstract int Hour { get; }

        /// <summary>
        /// derived class specifies the Minute (0-60) of the day when a trigger is fired
        /// </summary>
        protected abstract int Minute { get; }

        /// <summary>
        /// derived class specifies the days of week when a trigger is fired
        /// </summary>
        protected virtual List<DayOfWeek> DaysOfWeek { get; }

        protected override ITrigger CreateTriggerInternal()
        {

            var scheduler = (DaysOfWeek != null && DaysOfWeek.Any())
                ? CronScheduleBuilder.AtHourAndMinuteOnGivenDaysOfWeek(Hour, Minute, DaysOfWeek.ToArray())
                : CronScheduleBuilder.DailyAtHourAndMinute(Hour, Minute);

            scheduler.WithMisfireHandlingInstructionFireAndProceed();
            
            var trigger = TriggerBuilder.Create()
                .WithSchedule(scheduler)
                .Build();

            return trigger;
        }

        protected override IJobDetail CreateJobInternal()
        {
            var job = JobBuilder.Create<TJob>()
                .WithIdentity(JobIdentity, JobGroup)
                .Build();

            SetJobParams(job.JobDataMap);

            return job;
        }

        #endregion

    }
}
