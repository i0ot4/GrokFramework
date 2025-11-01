using Microsoft.Extensions.DependencyInjection;

namespace Grok.DependencyInjection
{
    public class GrokServiceCollection
    {
        public IServiceCollection Services;

        public GrokServiceCollection(IServiceCollection services)
        {
            Services = services;
        }
    }
}
