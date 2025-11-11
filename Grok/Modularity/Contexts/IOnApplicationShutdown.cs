using JetBrains.Annotations;

namespace Grok.Modularity.Contexts
{
    public interface IOnApplicationShutdown
    {
        Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context);

        void OnApplicationShutdown([NotNull] ApplicationShutdownContext context);
    }
}
