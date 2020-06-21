using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using Adacta.AdInsure.Framework.Core.Ioc;
using AuxiliaryService.Domain.IntegrationMap.Queries;
using AuxiliaryService.Domain.IntegrationMap.Repositories;
using AuxiliaryService.Domain.Notification.Queries;
using AuxiliaryService.Domain.Notification.Repositories;
using AuxiliaryService.Domain.Settings;
using AuxiliaryService.Domain.StateMachine.Queries;
using AuxiliaryService.Domain.StateMachine.Repositories;
using AuxiliaryService.Infrastructure.IntegrationMap.Queries;
using AuxiliaryService.Infrastructure.IntegrationMap.Repositories;
using AuxiliaryService.Infrastructure.Notification.Queries;
using AuxiliaryService.Infrastructure.Notification.Repositories;
using AuxiliaryService.Infrastructure.Repositories.StateMachine;
using AuxiliaryService.Infrastructure.StateMachine;
using AuxiliaryService.Infrastructure.StateMachine.Queries;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AuxiliaryService.Infrastructure
{
    public class SysInfrastructureServiceComponent : IocComponent
    {
        public override void Configure()
        {
            Repositories();
            Queries();
        }

        public override void Initialize()
        {
        }
        
        private void Repositories()
        {
            Bind<IStateMachineRepository>().To<StateMachineRepository>().InSingletonScope();
            Bind<INotificationRepository>().To<NotificationRepository>().InSingletonScope();
            Bind<IIntegrationMapRepository>().To<IntegrationMapRepository>().InSingletonScope();
        }

        private void Queries()
        {
            Bind<IStateMachineQueries>().To<StateMachineQueries>().InSingletonScope();
            Bind<INotificationQueries>().To<NotificationQueries>().InSingletonScope();
            Bind<IIntegrationMapQueries>().To<IntegrationMapQueries>().InSingletonScope();
        }

    }
}
