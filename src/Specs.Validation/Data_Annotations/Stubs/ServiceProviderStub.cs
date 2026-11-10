namespace Specs.DataAnnotations.Stubs;

internal sealed class ServiceProviderStub : Dictionary<Type, object>, IServiceProvider
{
    public object GetService(Type serviceType) => this[serviceType];
}
