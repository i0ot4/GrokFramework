using ConsoleApp;
using ConsoleApp.Services;
using Grok.Bootstrapper;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static void Main(string[] args)
    {

        Console.WriteLine("🚀 Starting Galaxy Application...");

        var app = GrokApplicationFactory.Create(typeof(ConsoleModule));
        app.Initialize();

        var helloService = app.ServiceProvider.GetService<IHelloService>();
        helloService?.SayHello();

        Console.WriteLine("✅ Application started successfully.");

        var cache1 = app.ServiceProvider.GetService<IAppCache>();
        var cache2 = app.ServiceProvider.GetService<IAppCache>();
        Console.WriteLine($"Singleton: {cache1.InstanceId == cache2.InstanceId}");

        using (var scope = app.ServiceProvider!.CreateScope())
        {
            var req1 = scope.ServiceProvider.GetService<IRequestTracker>();
            var req2 = scope.ServiceProvider.GetService<IRequestTracker>();
            Console.WriteLine($"Scoped (same scope): {req1!.ScopeId == req2!.ScopeId}");
            Console.WriteLine($"Scoped (new scope): {req1!.ScopeId}");
            Console.WriteLine($"Scoped (new scope): {req2!.ScopeId}");

        }

        using (var scope = app.ServiceProvider!.CreateScope())
        {
            var req3 = scope.ServiceProvider.GetService<IRequestTracker>();
            Console.WriteLine($"Scoped (new scope): {req3!.ScopeId}");
        }

        var rnd1 = app.ServiceProvider.GetService<IRandomizer>();
        var rnd2 = app.ServiceProvider.GetService<IRandomizer>();
        Console.WriteLine($"Transient: {rnd1.RandomId == rnd2.RandomId}");
        Console.WriteLine($"Transient (new Transient): {rnd1!.RandomId}");
        Console.WriteLine($"Transient (new Transient): {rnd2!.RandomId}");



    }
}