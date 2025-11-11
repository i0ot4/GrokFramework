using ConsoleApp.Services;
using Grok;
using Grok.Modularity;
using Grok.Modularity.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp
{
    public class CoreModule : GrokModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddSingleton<IHelloService, HelloService>();
            context.Services.AddSingleton<IAppCache, AppCache>();
            context.Services.AddScoped<IRequestTracker, RequestTracker>();
            context.Services.AddTransient<IRandomizer, Randomizer>();
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            Console.WriteLine("🧠 TestModule initialized!");
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            Console.WriteLine("🧹 TestModule shutting down!");
        }
    }


    [DependsOn(typeof(CoreModule))]
    public class ConsoleModule : GrokModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            base.ConfigureServices(context);
            Console.WriteLine("✅ ApplicationModule initialized (after CoreModule)");
        }
    }
}
