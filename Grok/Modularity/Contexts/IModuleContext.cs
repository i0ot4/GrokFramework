using Microsoft.Extensions.DependencyInjection;

namespace Grok.Modularity.Contexts
{
    public interface IModuleContext
    {
        IServiceCollection Services { get; }
    }
    public class ModuleContext : IModuleContext
    {
        public IServiceCollection Services { get; }

        public ModuleContext(IServiceCollection services)
        {
            Services = services;
        }
    }
}
