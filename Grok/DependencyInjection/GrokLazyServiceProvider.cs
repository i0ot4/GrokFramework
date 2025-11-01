namespace Grok.DependencyInjection
{
    public class GrokLazyServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public GrokLazyServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
