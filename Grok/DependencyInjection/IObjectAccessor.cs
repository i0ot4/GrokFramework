namespace Grok.DependencyInjection
{
    public interface IObjectAccessor<T>
    {
        T? Value { get; set; }
    }
}
