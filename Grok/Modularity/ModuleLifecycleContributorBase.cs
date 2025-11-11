using Grok.Modularity.Contexts;

namespace Grok.Modularity
{
    public abstract class ModuleLifecycleContributorBase : IModuleLifecycleContributor
    {
        public virtual Task InitializeAsync(ApplicationInitializationContext context, IGrokModule module)
        {
            return Task.CompletedTask;
        }

        public virtual void Initialize(ApplicationInitializationContext context, IGrokModule module)
        {
        }

        public virtual Task ShutdownAsync(ApplicationShutdownContext context, IGrokModule module)
        {
            return Task.CompletedTask;
        }

        public virtual void Shutdown(ApplicationShutdownContext context, IGrokModule module)
        {
        }
    }
}
