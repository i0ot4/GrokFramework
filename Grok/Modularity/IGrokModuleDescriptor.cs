using System.Reflection;

namespace Grok.Modularity
{

    public interface IGrokModuleDescriptor
    {
        Type Type { get; }

        Assembly Assembly { get; }

        Assembly[] AllAssemblies { get; }

        IGrokModule Instance { get; }

        IReadOnlyList<IGrokModuleDescriptor> Dependencies { get; }
    }
}
