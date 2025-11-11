using Grok.DependencyInjection;
using Grok.Modularity.Contexts;
using JetBrains.Annotations;

namespace Grok.Modularity
{
    public interface IModuleLifecycleContributor : ITransientDependency
    {
        Task InitializeAsync([NotNull] ApplicationInitializationContext context, [NotNull] IGrokModule module);

        void Initialize([NotNull] ApplicationInitializationContext context, [NotNull] IGrokModule module);

        Task ShutdownAsync([NotNull] ApplicationShutdownContext context, [NotNull] IGrokModule module);

        void Shutdown([NotNull] ApplicationShutdownContext context, [NotNull] IGrokModule module);
    }
}
