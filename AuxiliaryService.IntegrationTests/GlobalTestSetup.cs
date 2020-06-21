using Adacta.AdInsure.Framework.Core.ApplicationContext;
using Adacta.AdInsure.Framework.Core.AutoMapperConfiguration;
using Adacta.AdInsure.Framework.Core.Bootstraper;
using Adacta.AdInsure.Framework.Core.Cache;
using Adacta.AdInsure.Framework.Core.Common;
using Adacta.AdInsure.Framework.Core.Ioc;
using Adacta.AdInsure.Framework.Core.Ioc.Ninject;
using Adacta.AdInsure.Framework.Core.Localization;
using Adacta.AdInsure.Framework.Core.TestingInfrastructure.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections;
using ICache = Spring.Caching.ICache;
using Adacta.AdInsure.Framework.Core.TestingInfrastructure.Mocks;
using Adacta.AdInsure.Framework.Core.ScriptingEngine.Pool;
using Adacta.AdInsure.Framework.Core.SPI.UserGroup;
using Adacta.AdInsure.Framework.Core.SPI.UserProfile;
using Adacta.AdInsure.Framework.Core.SPI.Rendering;
using Adacta.AdInsure.Framework.Core.TestingInfrastructure.Fakes;

namespace AuxiliaryService.IntegrationTests
{
	[SetUpFixture]
    public class GlobalTestSetup
    {
        [OneTimeSetUp]
        public void GlobalSetup()
        {
            // Change factory to our custom TestKernel so we can guarantee the Single Responsibility Principle
            NinjectKernel.Reset();
            NinjectKernel.InstanceFactory = () => new TestKernel();

            var bootstrapper = new TestingBootstrapper(new IocModule[]
            {
                new FrameworkCoreModule()
            });

            NinjectKernel.Instance.Bind<IHostingEnvironment>().ToConstant(MockHostingEnviroment.GetHostingEnvironment());

            ApplicationContext.DataProvider = new TestApplicationContextDataProvider();

            NinjectKernel.Instance.Bind<ScriptingEnginePoolConfiguration>().ToConstant(new ScriptingEnginePoolConfiguration("fake-uninitialized-name"));
            NinjectKernel.Instance.Bind<IHostingEnvironment>().ToConstant(MockHostingEnviroment.GetHostingEnvironment());

            NinjectKernel.Instance.Bind<IUserProfileProvider>().ToConstant(new FakeUserProfileProvider());
            NinjectKernel.Instance.Bind<IUserGroupDataProvider>().ToConstant(new FakeUserGroupDataProvider());
            NinjectKernel.Instance.Bind<ITemplateProvider>().ToConstant(new FakeTemplateProvider());

            bootstrapper.Load(NinjectKernel.Instance);

            NinjectKernel.Instance.Bind<IAutoMapper>().To<Framework.Core.AutoMapperConfiguration.AutoMapper>();
        }

        class LocalizationServiceMock : ILocalizationService
        {
            public string Localize(string key, string defaultValue, params object[] arguments)
            {
                return arguments.Any() ? String.Format(defaultValue, arguments) : defaultValue;
            }

            public string LocalizeWithoutDefault(string key, params object[] arguments)
            {
                return arguments.Any() ? String.Format("{0} ({1})", key, string.Join(",", arguments)) : key;
            }
        }

        class LocalizationServiceProviderMock : ILocalizationServiceProvider
        {
            public string Localize(string key, string defaultValue, params object[] arguments)
            {
                return arguments.Any() ? String.Format(defaultValue, arguments) : defaultValue;
            }

            public string LocalizeWithoutDefault(string key, params object[] arguments)
            {
                return arguments.Any() ? String.Format("{0} ({1})", key, string.Join(",", arguments)) : key;
            }

            public ILocalizationService GetLocalizationService(string module)
            {
                return new LocalizationServiceMock();
            }
        }

        public class FakeCache : ICache
        {
            private readonly ConcurrentDictionary<string, object> _cache = new ConcurrentDictionary<string, object>();

            public int Count
            {
                get
                {
                    return _cache.Count;
                }
            }

            public ICollection Keys
            {
                get
                {
                    return (ICollection)_cache.Keys;
                }
            }

            public void Clear()
            {
                _cache.Clear();
            }

            public object Get(object key)
            {
                var stringKey = key.ToString();

                object val;
                if (!_cache.TryGetValue(stringKey, out val))
                {
                    return null;
                }

                return val;
            }

            public void Insert(object key, object value)
            {
                _cache[key.ToString()] = value;
            }

            public void Insert(object key, object value, TimeSpan timeToLive)
            {
                _cache[key.ToString()] = value;
            }

            public void Remove(object key)
            {
                object val;
                _cache.TryRemove(key.ToString(), out val);
            }

            public void RemoveAll(ICollection keys)
            {
                _cache.Clear();
            }
        }

        class FakeCacheProvider : ICacheProvider
        {
            private readonly ConcurrentDictionary<string, ICache> _cacheMap = new ConcurrentDictionary<string, ICache>();

            public void Clear(string name)
            {
                if (_cacheMap.ContainsKey(name))
                {
                    _cacheMap[name].Clear();
                }
            }

            public IDictionary<string, ICache> GetAllCaches()
            {
                return _cacheMap;
            }

            public ICache GetCache(string name)
            {
                return _cacheMap.GetOrAdd(name, new FakeCache());
            }
        }
    }
}