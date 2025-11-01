namespace Grok.DependencyInjection
{
    public interface IOnServiceRegistredContext
    {
        Type ImplementationType { get; }

        object? ServiceKey { get; }
    }
}
