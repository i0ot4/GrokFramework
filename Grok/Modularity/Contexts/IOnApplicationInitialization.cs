using JetBrains.Annotations;

namespace Grok.Modularity.Contexts
{
    public interface IOnApplicationInitialization
    {
        Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context);

        void OnApplicationInitialization([NotNull] ApplicationInitializationContext context);
    }
}
