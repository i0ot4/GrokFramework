using Grok.Bootstrapper;
using Grok.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Grok.Internal
{
    internal static class InternalServiceCollectionExtensions
    {
        internal static void AddCoreServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();
        }
        public static void AddCoreGrokServices(this IServiceCollection services,
            IGrokApplication application,
            GrokApplicationCreationOptions applicationCreationOptions)
        {
            services.TryAddSingleton<IModuleLoader>(new ModuleLoader());
            services.AddAssemblyOf<IGrokApplication>();
            services.Configure<GrokModuleLifecycleOptions>(options =>
            {
                options.Contributors.Add<OnPreApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnPostApplicationInitializationModuleLifecycleContributor>();
                options.Contributors.Add<OnApplicationShutdownModuleLifecycleContributor>();
            });
        }
    }
}
