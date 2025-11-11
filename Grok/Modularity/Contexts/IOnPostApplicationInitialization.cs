using JetBrains.Annotations;

namespace Grok.Modularity.Contexts
{
    public interface IOnPostApplicationInitialization
    {
        Task OnPostApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

        void OnPostApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}
