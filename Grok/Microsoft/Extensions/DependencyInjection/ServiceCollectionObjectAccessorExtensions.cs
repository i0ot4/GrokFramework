using Grok.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionObjectAccessorExtensions
    {
        public static IServiceCollection AddObjectAccessor<T>(this IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(IObjectAccessor<T>)))
            {
                services.AddSingleton<IObjectAccessor<T>, ObjectAccessor<T>>();
            }

            return services;
        }

        public static IServiceCollection AddObjectAccessor<T>(this IServiceCollection services, T obj)
        {
            if (!services.Any(s => s.ServiceType == typeof(IObjectAccessor<T>)))
            {
                services.AddSingleton<IObjectAccessor<T>>(new ObjectAccessor<T>(obj));
            }
            else
            {
                var provider = services.BuildServiceProvider();
                var accessor = provider.GetRequiredService<IObjectAccessor<T>>();
                accessor.Value = obj;
            }

            return services;
        }

        public static T? GetObjectOrNull<T>(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var accessor = provider.GetRequiredService<IObjectAccessor<T>>();
            return accessor.Value;
        }

        public static T GetRequiredObject<T>(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var accessor = provider.GetRequiredService<IObjectAccessor<T>>();
            return accessor.Value!;
        }

    }
}
