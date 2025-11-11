using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.Bootstrapper
{
    public static class GrokApplicationFactory
    {
        #region Internal
        public async static Task<IApplicationWithInternalServiceProvider> CreateAsync<TStartupModule>(
            Action<GrokApplicationCreationOptions>? optionsAction = null)
            where TStartupModule : IGrokModule
        {
            var app = Create<TStartupModule>(options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            });
            await app.ConfigureServicesAsync();
            return app;
        }

        public async static Task<IApplicationWithInternalServiceProvider> CreateAsync(
            [NotNull] Type startupType,
            Action<GrokApplicationCreationOptions>? optionAction = null)
        {
            var app = Create(startupType, option =>
            {
                option.SkipConfigureServices = true;
                optionAction?.Invoke(option);
            });
            await app.ConfigureServicesAsync();
            return app;
        }

        public static IApplicationWithInternalServiceProvider Create<TStartupModule>(
            Action<GrokApplicationCreationOptions>? optionAction = null)
            where TStartupModule : IGrokModule
        {
            return Create(typeof(TStartupModule), optionAction);
        }

        public static IApplicationWithInternalServiceProvider Create(
            [NotNull] Type startupType,
            Action<GrokApplicationCreationOptions>? optionAction = null)
        {
            return new ApplicationWithInternalServiceProvider(startupType, optionAction);
        }
        #endregion

        #region External
        public async static Task<IApplicationWithExternalServiceProvider> CreateAsync<TStartupModule>(
            [NotNull] IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionsAction = null) where TStartupModule : IGrokModule
        {
            var app = Create<TStartupModule>(services, options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            });
            await app.ConfigureServicesAsync();
            return app;
        }

        public static async Task<IApplicationWithExternalServiceProvider> CreateAsync(
            [NotNull] Type startupType,
            [NotNull] IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionsAction = null)
        {
            var app = new ApplicationWithExternalServiceProvider(startupType, services, options =>
            {
                options.SkipConfigureServices = true;
                optionsAction?.Invoke(options);
            });
            await app.ConfigureServicesAsync();
            return app;
        }

        public static IApplicationWithExternalServiceProvider Create<TStartupModule>(
            [NotNull] IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionsAction = null)
            where TStartupModule : IGrokModule
        {
            return Create(typeof(TStartupModule), services, optionsAction);
        }

        public static IApplicationWithExternalServiceProvider Create(
            [NotNull] Type startupType,
            [NotNull] IServiceCollection services,
            Action<GrokApplicationCreationOptions>? optionsAction = null)
        {
            return new ApplicationWithExternalServiceProvider(startupType, services, optionsAction);
        }

        #endregion
    }
}
