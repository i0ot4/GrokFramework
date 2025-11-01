namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static object? GetKeyedService(this IServiceProvider provider, Type serviceType, object? serviceKey)
        {
            if (provider is IKeyedServiceProvider keyedServiceProvider)
            {
                return keyedServiceProvider.GetKeyedService(serviceType, serviceKey);
            }

            throw new InvalidOperationException("KeyedServicesNotSupported");
        }
    }
}
