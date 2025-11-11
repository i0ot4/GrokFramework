using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.Bootstrapper
{
    public class ApplicationWithExternalServiceProvider : GrokApplicationBase, IApplicationWithExternalServiceProvider
    {
        public ApplicationWithExternalServiceProvider(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionsAction)
            : base(startupModuleType, services, optionsAction)
        {
            services.AddSingleton<IApplicationWithExternalServiceProvider>(this);
        }

        public void Initialize([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            SetServiceProvider(serviceProvider);

            InitializeModules();
        }

        public async Task InitializeAsync([NotNull] IServiceProvider serviceProvider)
        {
            Check.NotNull(InstanceId, nameof(InstanceId));

            SetServiceProvider(serviceProvider);
            await InitializeModulesAsync();
        }

        void IApplicationWithExternalServiceProvider.SetServiceProvider(IServiceProvider serviceProvider)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));

            if (ServiceProvider != null)
            {
                if (ServiceProvider != serviceProvider)
                {
                    throw new Exception("Service provider was already set before to another service provider instance.");
                }

                return;
            }

            SetServiceProvider(serviceProvider);
        }

        public override void Dispose()
        {
            base.Dispose();

            if (ServiceProvider is IDisposable disposableServiceProvider)
            {
                disposableServiceProvider.Dispose();
            }
        }
    }


}
