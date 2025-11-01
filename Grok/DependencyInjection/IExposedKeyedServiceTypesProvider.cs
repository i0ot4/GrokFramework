namespace Grok.DependencyInjection
{
    public interface IExposedKeyedServiceTypesProvider
    {
        ServiceIdentifier[] GetExposedServiceTypes(Type targetType);
    }
}