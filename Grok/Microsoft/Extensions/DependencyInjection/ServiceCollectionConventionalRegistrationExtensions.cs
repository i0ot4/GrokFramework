using Grok.Attributes;
using Grok.DependencyInjection;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionConventionalRegistrationExtensions
    {

        public static IServiceCollection AddConventionalRegistrar(this IServiceCollection services, IConventionalRegistrar registrar)
        {
            GetOrCreateRegistrarList(services).Add(registrar);
            return services;
        }

        public static List<IConventionalRegistrar> GetConventionalRegistrars(this IServiceCollection services)
        {
            return GetOrCreateRegistrarList(services);
        }

        private static ConventionalRegistrarList GetOrCreateRegistrarList(IServiceCollection services)
        {
            var conventionalRegistrars = services.GetSingletonInstanceOrNull<IObjectAccessor<ConventionalRegistrarList>>()?.Value;

            if (conventionalRegistrars == null)
            {
                conventionalRegistrars = new ConventionalRegistrarList { new DefaultConventionalRegistrar() };
                services.AddObjectAccessor(conventionalRegistrars);
            }

            return conventionalRegistrars;
        }

        public static IServiceCollection AddAssemblyOf<T>(this IServiceCollection services)
        {
            return services.AddAssembly(typeof(T).GetTypeInfo().Assembly);
        }

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            foreach (var registar in services.GetConventionalRegistrars())
            {
                registar.AddAssembly(services, assembly);
            }

            return services;
        }
        public static IServiceCollection AddTypes(this IServiceCollection services, params Type[] types)
        {
            foreach (var registar in services.GetConventionalRegistrars())
            {
                registar.AddTypes(services, types);
            }

            return services;
        }

        public static IServiceCollection AddType<TType>(this IServiceCollection services)
        {
            return services.AddType(typeof(TType));
        }

        public static IServiceCollection AddType(this IServiceCollection services, Type type)
        {
            foreach (var registar in services.GetConventionalRegistrars())
            {
                registar.AddType(services, type);
            }

            return services;
        }

        public static void AddByConvention(this IServiceCollection services, Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .ToList();

            foreach (var type in types)
            {
                var interfaces = type.GetInterfaces();

                //Singleton
                if (typeof(ISingletonDependency).IsAssignableFrom(type))
                {
                    RegisterService(services, type, interfaces, ServiceLifetime.Singleton);
                    continue;
                }

                // Transient
                if (typeof(ITransientDependency).IsAssignableFrom(type))
                {
                    RegisterService(services, type, interfaces, ServiceLifetime.Transient);
                    continue;
                }

                // Scoped
                if (typeof(IScopedDependency).IsAssignableFrom(type))
                {
                    RegisterService(services, type, interfaces, ServiceLifetime.Scoped);
                    continue;
                }


            }
        }

        private static void RegisterService(
            IServiceCollection services,
            Type implementationType,
            Type[] interfaces,
            ServiceLifetime lifetime)
        {
            var defaultInterface = interfaces.FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

            if (defaultInterface != null)
                services.Add(new ServiceDescriptor(defaultInterface, implementationType, lifetime));
            else
                services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));
        }

        private static void RegisterByDependencyInterfaces(IServiceCollection services, Type implementationType, Type lifetimeInterface)
        {
            var serviceLifetime = GetLifetimeFromInterface(lifetimeInterface);

            var exposeAttr = implementationType.GetCustomAttribute<ExposeServicesAttribute>();
            Type[] serviceTypes;

            if (exposeAttr != null && exposeAttr.ServiceTypes.Any())
            {
                serviceTypes = exposeAttr.ServiceTypes;
            }
            else
            {
                serviceTypes = implementationType.GetInterfaces()
                    .Where(i => i != lifetimeInterface &&
                    i != typeof(IDisposable) &&
                    !i.Name.Equals($"I{implementationType.Name}"))
                    .ToArray();

                if (serviceTypes.Length == 0)
                    serviceTypes = new[] { implementationType };
            }

            foreach (var serviceType in serviceTypes)
                services.Add(new ServiceDescriptor(serviceType, implementationType, serviceLifetime));

        }

        private static ServiceLifetime? GetLifetimeFromInterface(Type type)
        {
            if (typeof(ITransientDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Transient;
            }

            if (typeof(ISingletonDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Singleton;
            }

            if (typeof(IScopedDependency).GetTypeInfo().IsAssignableFrom(type))
            {
                return ServiceLifetime.Scoped;
            }

            return null;
        }
    }
}
