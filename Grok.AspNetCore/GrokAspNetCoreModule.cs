using Grok.Modularity.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Grok.AspNetCore
{
    public class GrokAspNetCoreModule : GrokModule
    {

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            //var abpHostEnvironment = context.Services.GetSingletonInstance<IGrokHostEnvironment>();
            //if (abpHostEnvironment.EnvironmentName.IsNullOrWhiteSpace())
            //{
            //    abpHostEnvironment.EnvironmentName = context.Services.GetHostingEnvironment().EnvironmentName;
            //}
        }
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAuthorization();

            AddAspNetServices(context.Services);
            context.Services.AddObjectAccessor<IApplicationBuilder>();

        }

        private static void AddAspNetServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
        }



    }
}
