using Microsoft.Extensions.DependencyInjection;

namespace Grok.DependencyInjection
{
    public interface IGrokServiceCollection
    {
        IServiceCollection Services { get; }
    }
}
