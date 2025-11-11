using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.Bootstrapper
{
    public class GrokApplicationCreationOptions
    {
        [NotNull]
        public IServiceCollection Services { get; }

        [NotNull]
        public GrokConfigurationBuilderOptions Configuration { get; }

        public bool SkipConfigureServices { get; set; }

        public string? ApplicationName { get; set; }

        public string? Environment { get; set; }

        public GrokApplicationCreationOptions([NotNull] IServiceCollection services)
        {
            Services = Check.NotNull(services, nameof(services));
            Configuration = new GrokConfigurationBuilderOptions();
        }
    }
}
