using Spectre.Console.Cli;
using Microsoft.Extensions.DependencyInjection;

namespace InkWell.Cli.Boilerplate;

public sealed class Registrar(IServiceCollection services) : ITypeRegistrar
{
    private readonly IServiceCollection _services = services;

    public void Register(Type service, Type implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void RegisterInstance(Type service, object instance)
    {
        _services.AddSingleton(service, instance);
    }

    public object BuildServiceProvider()
    {
        return _services.BuildServiceProvider();
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);

        _services.AddSingleton(service, sp => factory());
    }

    public ITypeResolver Build()
    {
        return new Resolver(_services.BuildServiceProvider());
    }
}