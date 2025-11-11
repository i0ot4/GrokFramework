using Grok.DependencyInjection;
using Grok.Internal;
using Grok.Modularity;
using Grok.Modularity.Contexts;
using Grok.System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Grok.Bootstrapper
{
    public abstract class GrokApplicationBase : IGrokApplication
    {

        [NotNull]
        public Type StartupModuleType { get; }

        public IServiceCollection Services { get; }

        public IServiceProvider ServiceProvider { get; private set; } = default!;

        public IReadOnlyList<IGrokModuleDescriptor> Modules { get; }

        public string? ApplicationName { get; }

        public string InstanceId { get; } = Guid.NewGuid().ToString();

        private bool _configuredServices;

        internal GrokApplicationBase(
        [NotNull] Type startupModuleType,
        [NotNull] IServiceCollection services,
        Action<GrokApplicationCreationOptions>? optionsAction)
        {
            Check.NotNull(startupModuleType, nameof(startupModuleType));
            Check.NotNull(services, nameof(services));

            StartupModuleType = startupModuleType;
            Services = services;

            services.TryAddObjectAccessor<IServiceProvider>();

            var options = new GrokApplicationCreationOptions(services);
            optionsAction?.Invoke(options);

            ApplicationName = GetApplicationName(options);

            services.AddSingleton<IGrokApplication>(this);
            services.AddSingleton<IApplicationInfoAccessor>(this);
            services.AddSingleton<IModuleContainer>(this);
            services.AddSingleton<IGrokHostEnvironment>(new GrokHostEnvironment()
            {
                EnvironmentName = options.Environment
            });

            Services.AddCoreServices();
            Services.AddCoreGrokServices(this, options);

            Modules = LoadModules(services);

            if (!options.SkipConfigureServices)
            {
                ConfigureServices();
            }
        }

        protected virtual void SetServiceProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ServiceProvider.GetRequiredService<ObjectAccessor<IServiceProvider>>().Value = ServiceProvider;
        }

        protected virtual IReadOnlyList<IGrokModuleDescriptor> LoadModules(IServiceCollection services)
        {
            return services.GetSingletonInstance<IModuleLoader>().LoadModules(services, StartupModuleType);
        }

        public static string? GetApplicationName(GrokApplicationCreationOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.ApplicationName))
            {
                return options.ApplicationName!;
            }

            var configration = options.Services.GetConfigurationOrNull();
            if (configration != null)
            {
                var appNameConfig = configration["ApplicationName"];
                if (!string.IsNullOrWhiteSpace(appNameConfig))
                {
                    return appNameConfig;
                }
            }

            var entryAssembly = Assembly.GetEntryAssembly();

            if (entryAssembly != null)
            {
                return entryAssembly.GetName().Name;
            }

            return null;
        }

        public virtual async Task ShutdownAsync()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .ShutdownModulesAsync(new ApplicationShutdownContext(scope.ServiceProvider));
            }
        }
        public virtual void Shutdown()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .ShutdownModules(new ApplicationShutdownContext(scope.ServiceProvider));
            }
        }

        protected virtual async Task InitializeModulesAsync()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                await scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModulesAsync(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        protected virtual void InitializeModules()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                scope.ServiceProvider
                    .GetRequiredService<IModuleManager>()
                    .InitializeModules(new ApplicationInitializationContext(scope.ServiceProvider));
            }
        }

        public virtual void Dispose()
        {

        }

        private void CheckMultipleConfigureServices()
        {
            if (_configuredServices)
            {
                throw new Exception("Services have already been configured! If you call ConfigureServicesAsync method, you must have set AbpApplicationCreationOptions.SkipConfigureServices to true before.");
            }
        }

        public virtual void ConfigureServices()
        {
            CheckMultipleConfigureServices();

            var context = new ServiceConfigurationContext(Services);
            Services.AddSingleton(context);

            foreach (var module in Modules)
            {
                if (module.Instance is GrokModule grokModule)
                {
                    grokModule.ServiceConfigurationContext = context;
                }
            }

            //PreConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
            {
                try
                {
                    ((IPreConfigureServices)module.Instance).PreConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            var assemblies = new HashSet<Assembly>();

            //ConfigureServices
            foreach (var module in Modules)
            {
                if (module.Instance is GrokModule grokModule)
                {
                    if (!grokModule.SkipAutoServiceRegistration)
                    {
                        foreach (var assembly in module.AllAssemblies)
                        {
                            if (!assemblies.Contains(assembly))
                            {
                                Services.AddAssembly(assembly);
                                assemblies.Add(assembly);
                            }
                        }
                    }
                }

                try
                {
                    module.Instance.ConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IGrokModule.ConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }

            }


            //PostConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
            {
                try
                {
                    ((IPostConfigureServices)module.Instance).PostConfigureServices(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServices)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            foreach (var module in Modules)
            {
                if (module.Instance is GrokModule grokModule)
                {
                    grokModule.ServiceConfigurationContext = null!;
                }
            }

            _configuredServices = true;
            TryToSetEnvironment(Services);

        }

        public async Task ConfigureServicesAsync()
        {
            CheckMultipleConfigureServices();
            var context = new ServiceConfigurationContext(Services);
            Services.AddSingleton(context);

            foreach (var module in Modules)
            {
                if (module.Instance is GrokModule grokModule)
                    grokModule.ServiceConfigurationContext = context;
            }



            //PreConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPreConfigureServices))
            {
                try
                {
                    await ((IPreConfigureServices)module.Instance).PreConfigureServicesAsync(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPreConfigureServices.PreConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            var assemblies = new HashSet<Assembly>();

            //ConfigureServices
            foreach (var module in Modules)
            {
                if (module.Instance is GrokModule grokModule)
                {
                    if (!grokModule.SkipAutoServiceRegistration)
                    {
                        foreach (var assembly in module.AllAssemblies)
                        {
                            if (!assemblies.Contains(assembly))
                            {
                                Services.AddAssembly(assembly);
                                assemblies.Add(assembly);
                            }
                        }
                    }
                }

                try
                {
                    await module.Instance.ConfigureServicesAsync(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IGrokModule.ConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }


            //PostConfigureServices
            foreach (var module in Modules.Where(m => m.Instance is IPostConfigureServices))
            {
                try
                {
                    await ((IPostConfigureServices)module.Instance).PostConfigureServicesAsync(context);
                }
                catch (Exception ex)
                {
                    throw new Exception($"An error occurred during {nameof(IPostConfigureServices.PostConfigureServicesAsync)} phase of the module {module.Type.AssemblyQualifiedName}. See the inner exception for details.", ex);
                }
            }

            foreach (var module in Modules)
            {
                if (module.Instance is GrokModule grokModule)
                {
                    grokModule.ServiceConfigurationContext = null!;
                }
            }

            _configuredServices = true;

            TryToSetEnvironment(Services);
        }


        private static void TryToSetEnvironment(IServiceCollection services)
        {
            var abpHostEnvironment = services.GetSingletonInstance<IGrokHostEnvironment>();
            if (abpHostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
            {
                abpHostEnvironment.EnvironmentName = Environments.Production;
            }
        }
    }

}
