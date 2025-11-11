namespace Grok.Modularity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute, IDependedTypesProvider
    {
        public Type[] DependedTypes { get; }
        public DependsOnAttribute(params Type[] dependedTypes) => DependedTypes = dependedTypes;

        public virtual Type[] GetDependedTypes()
        {
            return DependedTypes;
        }
    }
}
