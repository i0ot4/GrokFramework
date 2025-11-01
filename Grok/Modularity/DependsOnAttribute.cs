namespace Grok.Modularity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        public Type[] DependsOnTypes { get; }
        public DependsOnAttribute(params Type[] dependsOnTypes) => DependsOnTypes = dependsOnTypes;
    }
}
