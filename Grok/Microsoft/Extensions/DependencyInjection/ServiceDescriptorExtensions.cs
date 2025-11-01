namespace Microsoft.Extensions.DependencyInjection
{

    /// <summary>
    /// Extensions for working with <see cref="ServiceDescriptor"/>.
    /// </summary>
    public static class ServiceDescriptorExtensions
    {
        public static object? NormalizedImplementationInstance(this ServiceDescriptor descriptor)
        {
            return descriptor.IsKeyedService ? descriptor.KeyedImplementationInstance : descriptor.ImplementationInstance;
        }

        /// <summary>
        /// Normalizes the implementation type data between keyed and not keyed services.
        /// </summary>
        /// <param name="descriptor">
        /// The <see cref="ServiceDescriptor"/> to normalize.
        /// </param>
        /// <returns>
        /// The appropriate implementation type from the service descriptor.
        /// </returns>
        public static Type? NormalizedImplementationType(this ServiceDescriptor descriptor)
            => descriptor.IsKeyedService ? descriptor.KeyedImplementationType : descriptor.ImplementationType;
    }
}
