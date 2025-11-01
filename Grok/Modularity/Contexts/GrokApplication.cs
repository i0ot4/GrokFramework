using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Grok.Modularity.Contexts
{
    public interface IGrokApplication : IDisposable, IAsyncDisposable
    {

    }
    public class GrokApplication : IGrokApplication
    {
        private bool _isDisposed;
        public IServiceCollection Services { get; }
        public IServiceProvider? ServiceProvider { get; private set; }

        public GrokApplication(IServiceCollection? services = null)
        {
            Services = services ?? new ServiceCollection();
        }

        public void Initialize(Assembly entryAssembly)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(GrokApplication));

            ModuleLoader.LoadModules(entryAssembly, Services);

            ServiceProvider = Services.BuildServiceProvider();
        }

        public T GetService<T>() where T : notnull
        {

            if (_isDisposed)
                throw new ObjectDisposedException(nameof(GrokApplication));

            if (ServiceProvider == null)
                throw new InvalidOperationException("The application has not been initialized.");

            return ServiceProvider.GetRequiredService<T>();
        }

        public void Dispose()
        {
            if (_isDisposed) return;

            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed) return;

            if (ServiceProvider is IAsyncDisposable asyncDisposable)
                await asyncDisposable.DisposeAsync();
            else if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
