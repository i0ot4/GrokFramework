using Grok.Modularity.Contexts;
using JetBrains.Annotations;

namespace Grok.Modularity
{
    public interface IModuleManager
    {
        Task InitializeModulesAsync([NotNull] ApplicationInitializationContext context);

        void InitializeModules([NotNull] ApplicationInitializationContext context);

        Task ShutdownModulesAsync([NotNull] ApplicationShutdownContext context);

        void ShutdownModules([NotNull] ApplicationShutdownContext context);
    }
}
