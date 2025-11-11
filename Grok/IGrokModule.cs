using Grok.Modularity.Contexts;

namespace Grok
{
    public interface IGrokModule
    {
        Task ConfigureServicesAsync(ServiceConfigurationContext context);

        void ConfigureServices(ServiceConfigurationContext context);
    }
}