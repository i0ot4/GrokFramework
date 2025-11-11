using Grok.DependencyInjection;
using Grok.Modularity.Contexts;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Grok.Modularity
{
    public class ModuleManager : IModuleManager, ISingletonDependency
    {
        private readonly IModuleContainer _moduleContainer;
        private readonly IEnumerable<IModuleLifecycleContributor> _lifecycleContributors;
        private readonly ILogger<ModuleManager> _logger;

        public ModuleManager(
            IModuleContainer moduleContainer,
            IOptions<GrokModuleLifecycleOptions> options,
            ILogger<ModuleManager> logger,
            IServiceProvider serviceProvider)
        {
            _moduleContainer = moduleContainer;
            _logger = logger;

            _lifecycleContributors = options.Value
                .Contributors.Select(serviceProvider.GetRequiredService)
                .Cast<IModuleLifecycleContributor>()
                .ToArray();
        }

        public async Task InitializeModulesAsync([NotNull] ApplicationInitializationContext context)
        {
            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        await contributor.InitializeAsync(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
            _logger.LogInformation("Initialized all Grok modules.");
        }

        public void InitializeModules([NotNull] ApplicationInitializationContext context)
        {
            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in _moduleContainer.Modules)
                {
                    try
                    {
                        contributor.Initialize(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the initialize {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }

            _logger.LogInformation("Initialized all Grok modules.");
        }

        public void ShutdownModules([NotNull] ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();
            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in modules)
                {
                    try
                    {
                        contributor.Shutdown(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }

        public async Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context)
        {
            var modules = _moduleContainer.Modules.Reverse().ToList();

            foreach (var contributor in _lifecycleContributors)
            {
                foreach (var module in modules)
                {
                    try
                    {
                        await contributor.ShutdownAsync(context, module.Instance);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"An error occurred during the shutdown {contributor.GetType().FullName} phase of the module {module.Type.AssemblyQualifiedName}: {ex.Message}. See the inner exception for details.", ex);
                    }
                }
            }
        }
    }
}
