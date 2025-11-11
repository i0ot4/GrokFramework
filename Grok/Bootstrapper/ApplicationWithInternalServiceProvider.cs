using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.Bootstrapper
{

    public class ApplicationWithInternalServiceProvider : GrokApplicationBase, IApplicationWithInternalServiceProvider
    {
        public IServiceScope? ServiceScope { get; private set; }

        public ApplicationWithInternalServiceProvider(
            [NotNull] Type startupModuleType,
            Action<GrokApplicationCreationOptions>? optionsAction)
            : this(startupModuleType, new ServiceCollection(), optionsAction) { }

        public ApplicationWithInternalServiceProvider(
            [NotNull] Type startupModuleType,
            [NotNull] IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionsAction) : base(
                startupModuleType,
                services,
                optionsAction)
        {
            Services.AddSingleton<IApplicationWithInternalServiceProvider>(this);
        }

        public IServiceProvider CreateServiceProvider()
        {
            if (ServiceProvider != null)
            {
                return ServiceProvider;
            }

            ServiceScope = Services.BuildServiceProviderFromFactory().CreateScope();
            SetServiceProvider(ServiceScope.ServiceProvider);

            return ServiceProvider!;
        }

        public void Initialize()
        {
            CreateServiceProvider();
            InitializeModules();
        }

        public async Task InitializeAsync()
        {
            CreateServiceProvider();
            await InitializeModulesAsync();
        }

        public override void Dispose()
        {
            base.Dispose();
            ServiceScope?.Dispose();
        }

    }


}
