using Grok.System;
using JetBrains.Annotations;
using System.Collections.Immutable;
using System.Reflection;

namespace Grok.Modularity
{
    public class GrokModuleDescriptor : IGrokModuleDescriptor
    {
        public Type Type { get; }

        public Assembly Assembly { get; }

        public Assembly[] AllAssemblies { get; }

        public IGrokModule Instance { get; }

        private readonly List<IGrokModuleDescriptor> _dependencies = [];
        public IReadOnlyList<IGrokModuleDescriptor> Dependencies => _dependencies.ToImmutableList();

        public GrokModuleDescriptor(
            [NotNull] Type type,
            [NotNull] IGrokModule instance)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(instance, nameof(instance));
            GrokModule.CheckGrokModuleType(type);

            if (!type.GetTypeInfo().IsAssignableFrom(instance.GetType()))
            {
                throw new ArgumentException($"Given module instance ({instance.GetType().AssemblyQualifiedName}) is not an instance of given module type: {type.AssemblyQualifiedName}");
            }

            Type = type;
            Assembly = type.Assembly;
            AllAssemblies = GrokModuleHelper.GetAllAssemblies(type);
            Instance = instance;

            _dependencies = new List<IGrokModuleDescriptor>();
        }

        public void AddDependency(IGrokModuleDescriptor descriptor)
        {
            _dependencies.AddIfNotContains(descriptor);
        }

        public override string ToString()
        {
            return $"[AbpModuleDescriptor {Type.FullName}]";
        }
    }
}
