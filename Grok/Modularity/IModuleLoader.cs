using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.Modularity
{
    public interface IModuleLoader
    {
        [NotNull]
        IGrokModuleDescriptor[] LoadModules(
        [NotNull] IServiceCollection services,
        [NotNull] Type startupModuleType);
    }
}