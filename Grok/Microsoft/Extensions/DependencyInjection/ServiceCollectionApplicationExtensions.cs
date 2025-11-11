using Grok;
using Grok.Bootstrapper;
using JetBrains.Annotations;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionApplicationExtensions
    {

        public static IApplicationWithExternalServiceProvider AddApplication<TStartupModule>(
            [NotNull] this IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionAction = null)
            where TStartupModule : IGrokModule
        {
            return GrokApplicationFactory.Create<TStartupModule>(services, optionAction);
        }

        public static IApplicationWithExternalServiceProvider AddApplication(
            [NotNull] this IServiceCollection services,
            [NotNull] Type startupModuleType,
            Action<GrokApplicationCreationOptions>? optionAction = null)
        {
            return GrokApplicationFactory.Create(startupModuleType, services, optionAction);
        }

        public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
            [NotNull] this IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionAction = null)
            where TStartupModule : IGrokModule
        {
            return await GrokApplicationFactory.CreateAsync<TStartupModule>(services, optionAction);
        }

        public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync(
            [NotNull] this IServiceCollection services,
            [NotNull] Type startupModuleType,
            Action<GrokApplicationCreationOptions>? optionAction = null)
        {
            return await GrokApplicationFactory.CreateAsync(startupModuleType, services, optionAction);
        }
    }
}
