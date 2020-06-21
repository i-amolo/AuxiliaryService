using AuxiliaryService.Domain.Notification;
using AuxiliaryService.Domain.Notification.Services;
using System;
using System.Collections.Concurrent;

namespace AuxiliaryService.DomainService.Notification.Services
{
	public class NotificationProviderResolver : INotificationProviderResolver
    {
        private readonly ConcurrentDictionary<string, INotificationProvider> _mapping = new ConcurrentDictionary<string, INotificationProvider>();

        public void Register(INotificationProvider provider, string providerType)
        {
            if (!_mapping.ContainsKey(providerType))
                _mapping[providerType] = provider;
        }

        public INotificationProvider<TProviderDetails> Resolve<TProviderDetails>(string providerType) where TProviderDetails : NotificationProviderDetailsDomainBase
        {
            if (!_mapping.ContainsKey(providerType))
                throw new Exception($"Notification provider for type {providerType} isn't found");

            var provider = _mapping[providerType] as INotificationProvider<TProviderDetails> 
                    ?? throw new Exception($"Notification provider registered for type {providerType} doens't correspond to a value of {nameof(TProviderDetails)} parameter");

            return provider;
        }
    }
}