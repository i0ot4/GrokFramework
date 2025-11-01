using Grok.Attributes;
using Grok.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleApp.Services
{
    public interface IHelloServiceA
    {
        void SayNotHello();
    }
    public interface IHelloService
    {
        void SayHello();
    }

    [Dependency(ServiceLifetime.Singleton)]
    public class HelloService : IHelloService, IHelloServiceA
    {
        public void SayHello() => Console.WriteLine("👋 Hello from convention-based registration!");

        public void SayNotHello() => Console.WriteLine("👋 Not Hello from convention-based registration!");
    }



    public interface IAppCache
    {
        Guid InstanceId { get; }
    }

    public class AppCache : IAppCache, ISingletonDependency
    {
        public Guid InstanceId { get; } = Guid.NewGuid();
    }



    public interface IRequestTracker
    {
        Guid ScopeId { get; }
    }

    public class RequestTracker : IRequestTracker, IScopedDependency
    {
        public Guid ScopeId { get; } = Guid.NewGuid();
    }




    public interface IRandomizer : ITransientDependency
    {
        Guid RandomId { get; }
    }

    public class Randomizer : IRandomizer
    {
        public Guid RandomId { get; } = Guid.NewGuid();
    }

}
