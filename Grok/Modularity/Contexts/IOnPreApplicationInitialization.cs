using JetBrains.Annotations;

namespace Grok.Modularity.Contexts
{
    public interface IOnPreApplicationInitialization
    {
        Task OnPreApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

        void OnPreApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}
