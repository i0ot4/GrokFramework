namespace Grok.Modularity.Contexts
{
    public interface IPostConfigureServices
    {
        Task PostConfigureServicesAsync(ServiceConfigurationContext context);

        void PostConfigureServices(ServiceConfigurationContext context);
    }
}
