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
    public abstract class SimpleTriggerBatchJobConfigurationBase<TJob> : ScheduleBatchJobConfigurationBase<TJob>
        where TJob: IJob
    {

        #region protected

        /// <summary>
        /// derived class specifies if the trigger should start immediately 
        /// </summary>
        protected virtual bool StartNow => true;

        /// <summary>
        /// derived class specifies if the trigger should start immediately 
        /// </summary>
        protected virtual DateTimeOffset? StartAt => null;

        /// <summary>
        /// derived class specifies interval in minutes
        /// </summary>
        protected virtual int? IntervalInMinutes => null;

        /// <summary>
        /// derived class specifies interval in minutes
        /// </summary>
        protected virtual int? IntervalInHours => null;

        /// <summary>
        /// derived class specifies how many time jb is fired
        /// if null than forever
        /// </summary>
        protected virtual int? RepeatCount => null;

        /// <summary>
        /// derived class specifies interval in minutes
        /// </summary>
        protected virtual int? IntervalInSeconds => null;

        protected override ITrigger CreateTriggerInternal()
        {
            var triggerBldr = TriggerBuilder.Create();

            if (StartNow)
            {
                triggerBldr.StartNow();
            }
            else
            {
                triggerBldr = StartAt != null
                    ? triggerBldr.StartAt(StartAt.Value)
                    : throw new Exception($"Trigger configuration failed. StartAt isn't specified. Class: {this.GetType().Name}");
            }

            if (IntervalInHours != null)
                triggerBldr = triggerBldr.WithSimpleSchedule(s => 
                {
                    var sb = s.WithIntervalInHours(IntervalInHours.Value);

                    if (RepeatCount != null)
                        sb.WithRepeatCount(RepeatCount.Value);
                    else
                        sb.RepeatForever();

                });
            else if (IntervalInMinutes != null)
                triggerBldr = triggerBldr.WithSimpleSchedule(s =>
                {
                    var sb = s.WithIntervalInMinutes(IntervalInMinutes.Value);

                    if (RepeatCount != null)
                        sb.WithRepeatCount(RepeatCount.Value);
                    else
                        sb.RepeatForever();
                });
            else if (IntervalInSeconds != null)
                triggerBldr = triggerBldr.WithSimpleSchedule(s =>
                {
                    var sb = s.WithIntervalInSeconds(IntervalInSeconds.Value);
                    if (RepeatCount != null)
                        sb.WithRepeatCount(RepeatCount.Value);
                    else
                        sb.RepeatForever();
                });
            else
                throw new Exception($"Trigger configuration failed. Interval isn't specified. Class: {this.GetType().Name}");

            var trigger = triggerBldr.Build();

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
