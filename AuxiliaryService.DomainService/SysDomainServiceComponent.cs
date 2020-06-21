using Adacta.AdInsure.Framework.Core.Ioc;
using Adacta.AdInsure.Framework.Core.ConfigurationSettings.Interfaces;
using Ninject;
using AuxiliaryService.Domain.Notification.Services;
using AuxiliaryService.DomainService.Notification.Services;
using AuxiliaryService.Domain.Notification.Providers.Email;
using AuxiliaryService.DomainService.Notification.Services.Providers.Email;
using AuxiliaryService.Domain.Notification.Consts;
using AuxiliaryService.Domain.Notification.Providers.Email.Settings;
using AuxiliaryService.Domain.Settings;

namespace AuxiliaryService.Domain
{
    public class SysDomainServiceComponent : IocComponent
    {
        public override void Configure()
        {
            Bind<IEmailNotificationSettingsFactory>().To<EmailNotificationSettingsFactory>().InSingletonScope();
            Bind<IEmailNotificationSettings>()
                .ToMethod(k => k.Kernel.Get<IConfigurationSettingRegistrator>().GetSettings<EmailNotificationSettings>())
                .InSingletonScope();

            Bind<ISystemSettingsFactory>().To<SystemSettingsFactory>().InSingletonScope();
            Bind<ISystemSettings>()
                .ToMethod(k => k.Kernel.Get<IConfigurationSettingRegistrator>().GetSettings<SystemSettings>())
                .InSingletonScope();

            Bind<INotificationProviderResolver>().To<NotificationProviderResolver>().InSingletonScope();
            Bind<INotificationDomainService>().To<NotificationDomainService>().InSingletonScope();

            Bind<INotificationProvider<EmailNotificationProviderDetailsDomain>>().To<EmailNotificationProvider>().InSingletonScope();
        }

        public override void Initialize()
        {
            var resolver = KernelInstance.Get<INotificationProviderResolver>();
            var emailNotificationProvider = KernelInstance.Get<INotificationProvider<EmailNotificationProviderDetailsDomain>>();
            resolver.Register(emailNotificationProvider, NotificationProviderTypeConsts.Email);
        }
    }
}