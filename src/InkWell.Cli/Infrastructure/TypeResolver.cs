using Spectre.Console.Cli;

namespace InkWell.Cli.Infrastructure;

public class TypeResolver(IServiceProvider provider) : ITypeResolver, IDisposable
{
    public void Dispose()
    {
        if (provider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    public object? Resolve(Type? type)
    {
        if (type == null)
        {
            return null;
        }

        return provider.GetService(type);
    }
}
