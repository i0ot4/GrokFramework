namespace Grok.DependencyInjection
{
    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        public ObjectAccessor()
        {

        }

        public T? Value { get; set; }

        public ObjectAccessor(T value)
        {
            Value = value;
        }
    }
}
