using Grok;
using Grok.Modularity.Contexts;
using JetBrains.Annotations;

namespace DemoWebAPI
{
    public class DemoWebApiModule : GrokModule
    {

        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            context.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            context.Services.AddOpenApi();

            base.ConfigureServices(context);
        }


        public override void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {

            base.OnApplicationInitialization(context);
        }
    }
}
