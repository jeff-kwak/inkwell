using Spectre.Console.Cli;
using Microsoft.Extensions.DependencyInjection;

namespace InkWell.Cli.Boilerplate;

public sealed class Resolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    private readonly IServiceProvider _provider = provider;

    public object? Resolve(Type? type)
    {
        if (type is null) return null;

        return _provider.GetRequiredService(type);
    }

    public void Dispose()
    {
        if (_provider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}