using Grok.Modularity.Contexts;

namespace Grok.Modularity
{
    public class OnApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public override void Initialize(ApplicationInitializationContext context, IGrokModule module)
        {
            if (module is IOnApplicationInitialization onApplicationInitialization)
            {
                onApplicationInitialization.OnApplicationInitialization(context);
            }
        }

        public override async Task InitializeAsync(ApplicationInitializationContext context, IGrokModule module)
        {
            if (module is IOnApplicationInitialization onApplicationInitialization)
            {
                await onApplicationInitialization.OnApplicationInitializationAsync(context);
            }
        }
    }

    public class OnApplicationShutdownModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public async override Task ShutdownAsync(ApplicationShutdownContext context, IGrokModule module)
        {
            if (module is IOnApplicationShutdown onApplicationShutdown)
            {
                await onApplicationShutdown.OnApplicationShutdownAsync(context);
            }
        }

        public override void Shutdown(ApplicationShutdownContext context, IGrokModule module)
        {
            (module as IOnApplicationShutdown)?.OnApplicationShutdown(context);
        }
    }

    public class OnPreApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public async override Task InitializeAsync(ApplicationInitializationContext context, IGrokModule module)
        {
            if (module is IOnPreApplicationInitialization onPreApplicationInitialization)
            {
                await onPreApplicationInitialization.OnPreApplicationInitializationAsync(context);
            }
        }

        public override void Initialize(ApplicationInitializationContext context, IGrokModule module)
        {
            (module as IOnPreApplicationInitialization)?.OnPreApplicationInitialization(context);
        }
    }

    public class OnPostApplicationInitializationModuleLifecycleContributor : ModuleLifecycleContributorBase
    {
        public async override Task InitializeAsync(ApplicationInitializationContext context, IGrokModule module)
        {
            if (module is IOnPostApplicationInitialization onPostApplicationInitialization)
            {
                await onPostApplicationInitialization.OnPostApplicationInitializationAsync(context);
            }
        }

        public override void Initialize(ApplicationInitializationContext context, IGrokModule module)
        {
            (module as IOnPostApplicationInitialization)?.OnPostApplicationInitialization(context);
        }
    }
}
