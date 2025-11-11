using Grok.Bootstrapper;
using Grok.System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.AspNetCore.Microsoft.Extensions
{
    public static class WebApplicationBuilderExtensions
    {

        public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync(
        [NotNull] this WebApplicationBuilder builder,
        [NotNull] Type startupModuleType,
        Action<GrokApplicationCreationOptions>? optionsAction = null)
        {
            return await builder.Services.AddApplicationAsync(startupModuleType, options =>
            {
                options.Services.ReplaceConfiguration(builder.Configuration);
                optionsAction?.Invoke(options);
                if (options.Environment.IsNullOrWhiteSpace())
                {
                    options.Environment = builder.Environment.EnvironmentName;
                }
            });
        }

        public static async Task<IApplicationWithExternalServiceProvider> AddApplicationAsync<TStartupModule>(
        [NotNull] this WebApplicationBuilder builder,
        Action<GrokApplicationCreationOptions>? optionsAction = null)
        where TStartupModule : IGrokModule
        {
            return await builder.Services.AddApplicationAsync<TStartupModule>(options =>
            {
                options.Services.ReplaceConfiguration(builder.Configuration);
                optionsAction?.Invoke(options);
                if (options.Environment.IsNullOrWhiteSpace())
                {
                    options.Environment = builder.Environment.EnvironmentName;
                }
            });
        }
    }
}
