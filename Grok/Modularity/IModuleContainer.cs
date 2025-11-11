namespace Grok.Modularity
{
    public interface IModuleContainer
    {
        IReadOnlyList<IGrokModuleDescriptor> Modules { get; }
    }
}
