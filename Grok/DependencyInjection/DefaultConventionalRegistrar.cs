using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Grok.DependencyInjection
{
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {
        public override void AddType(IServiceCollection services, Type type)
        {

            if (IsConventionalRegistrationDisabled(type)) return;

            var dependencyAttribute = GetDependencyAttributeOrNull(type);
            var lifeTime = GetLifeTimeOrNull(type, dependencyAttribute);

            if (lifeTime is null) return;

            var exposedServiceAndKeyedServiceTypes = GetExposedKeyedServiceTypes(type)
                .Concat(GetExposedServiceTypes(type).Select(t => new ServiceIdentifier(t))).ToList();

            TriggerServiceExposing(services, type, exposedServiceAndKeyedServiceTypes);

            foreach (var exposedServiceType in exposedServiceAndKeyedServiceTypes)
            {
                //if (type.IsAbstract || type.IsInterface) continue;

                var allExposingServiceTypes = exposedServiceType.ServiceType is null ?
                    exposedServiceAndKeyedServiceTypes.Where(k => k.ServiceKey is null).ToList()
                    : exposedServiceAndKeyedServiceTypes.Where(k => k.ServiceKey?.ToString() == (exposedServiceType.ServiceKey?.ToString())).ToList();

                var serviceDescriptor = CreateServiceDescriptor(
                    type,
                    exposedServiceType.ServiceKey,
                    exposedServiceType.ServiceType,
                    allExposingServiceTypes,
                    lifeTime.Value);

                if (dependencyAttribute?.ReplaceServices == true) services.Replace(serviceDescriptor);
                else if (dependencyAttribute?.TryRegister == true) services.TryAdd(serviceDescriptor);
                else services.Add(serviceDescriptor);

            }


        }
    }
}
