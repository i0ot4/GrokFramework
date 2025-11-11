namespace Grok.Modularity.Contexts
{
    public interface IPreConfigureServices
    {
        Task PreConfigureServicesAsync(ServiceConfigurationContext context);

        void PreConfigureServices(ServiceConfigurationContext context);
    }
}
