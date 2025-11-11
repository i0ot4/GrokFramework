using Grok.Modularity.Contexts;
using JetBrains.Annotations;

namespace Grok
{
    public abstract class GrokModule : IGrokModule,
        IPostConfigureServices,
        IPreConfigureServices,
        IOnPreApplicationInitialization,
        IOnPostApplicationInitialization,
        IOnApplicationInitialization,
        IOnApplicationShutdown
    {
        protected internal bool SkipAutoServiceRegistration { get; protected set; }
        private ServiceConfigurationContext? _serviceConfigurationContext;

        protected internal ServiceConfigurationContext ServiceConfigurationContext
        {
            get
            {
                if (_serviceConfigurationContext == null)
                {
                    throw new Exception($"{nameof(ServiceConfigurationContext)} is only available in the {nameof(ConfigureServices)}, {nameof(PreConfigureServices)} and {nameof(PostConfigureServices)} methods.");
                }

                return _serviceConfigurationContext;
            }
            internal set => _serviceConfigurationContext = value;
        }

        public static void CheckGrokModuleType([NotNull] Type type)
        {
            Check.NotNull(type, nameof(type));

            if (!typeof(IGrokModule).IsAssignableFrom(type))
            {
                throw new ArgumentException($"Given type ({type.AssemblyQualifiedName}) is not an Grok module. All Grok modules must implement {typeof(IGrokModule).AssemblyQualifiedName} interface.");
            }
        }


        public virtual Task PreConfigureServicesAsync(ServiceConfigurationContext context)
        {
            PreConfigureServices(context);
            return Task.CompletedTask;
        }

        public virtual void PreConfigureServices(ServiceConfigurationContext context)
        {

        }

        public virtual Task ConfigureServicesAsync(ServiceConfigurationContext context)
        {
            ConfigureServices(context);
            return Task.CompletedTask;
        }

        public virtual void ConfigureServices(ServiceConfigurationContext context)
        {

        }

        public virtual Task PostConfigureServicesAsync(ServiceConfigurationContext context)
        {
            PostConfigureServices(context);
            return Task.CompletedTask;
        }

        public virtual void PostConfigureServices(ServiceConfigurationContext context)
        {

        }

        public virtual Task OnPreApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            OnPreApplicationInitialization(context);
            return Task.CompletedTask;
        }

        public virtual void OnPreApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {

        }

        public virtual Task OnPostApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            OnPostApplicationInitialization(context);
            return Task.CompletedTask;
        }

        public virtual void OnPostApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {

        }

        public virtual Task OnApplicationInitializationAsync([NotNull] ApplicationInitializationContext context)
        {
            OnApplicationInitialization(context);
            return Task.CompletedTask;
        }

        public virtual void OnApplicationInitialization([NotNull] ApplicationInitializationContext context)
        {

        }

        public virtual Task OnApplicationShutdownAsync([NotNull] ApplicationShutdownContext context)
        {
            OnApplicationShutdown(context);
            return Task.CompletedTask;
        }

        public virtual void OnApplicationShutdown([NotNull] ApplicationShutdownContext context)
        {

        }



        public static bool IsAbpModule(Type type)
        {
            var typeInfo = type.GetType();

            return typeInfo.IsClass &&
                !typeInfo.IsAbstract &&
                !typeInfo.IsGenericType &&
                typeof(IGrokModule).GetType().IsAssignableFrom(typeInfo);
        }
    }

}
