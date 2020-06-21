using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using Adacta.AdInsure.Framework.Core.Ioc;
using AuxiliaryService.Scheduler.API;
using AuxiliaryService.Scheduler.Contract;
using AuxiliaryService.Scheduler.Infrastructure;
using Ninject;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AuxiliaryService.Scheduler
{
    public class SysSchedulerComponent : IocComponent
    {
        public override void Configure()
        {

            Rebind<IJobFactory>().To<NinjectJobFactory>().InSingletonScope();
            Rebind<ISchedulerFactory>().ToConstant(new StdSchedulerFactory());

            Bind<ISchedulerFactoryConfiguration>().To<SchedulerFactoryConfiguration>().InSingletonScope();
            Bind<IBatchJobRunner>().To<BatchJobRunner>().InSingletonScope();
            
        }

        public override void Initialize()
        {

            // workaround to avoid using core configurations
            // we can put custom configuration here
            var schedulerFactory = KernelInstance.Get<ISchedulerFactory>() as StdSchedulerFactory;
            schedulerFactory.Initialize(KernelInstance.Get<ISchedulerFactoryConfiguration>().GetConfiguration());

            Rebind<IScheduler>().ToMethod(ctx =>
            {
                var scheduler = schedulerFactory.GetScheduler().Result;
                scheduler.JobFactory = KernelInstance.Get<IJobFactory>();
                return scheduler;
            }).InSingletonScope();

            var runner = KernelInstance.Get<IBatchJobRunner>();
            runner.RegisterBatchJobs(KernelInstance.GetAll<IBatchJobConfiguration>());
            runner.Start();

        }

    }
}
