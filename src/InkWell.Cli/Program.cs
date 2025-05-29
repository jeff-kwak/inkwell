using InkWell.Cli.Commands;
using InkWell.Cli.Core;
using InkWell.Cli.Infrastructure;
using InkWell.Cli.Tools;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();
services.AddSingleton<IDirectoryTool, DirectoryTool>();
services.AddSingleton<IFileTool, FileTool>();
services.AddSingleton<ITemplateLoader, TemplateLoader>();
services.AddSingleton<IContentInfo, SiteInfo>();

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("inkwell")
        .SetApplicationVersion("0.1.0");

    config.AddCommand<CompileCommand>("compile")
        .WithAlias("c")
        .WithDescription("Compiles the source InkWell directory into a static HTML site")
        .WithExample(["compile", "path/to/source", "path/to/output"]);

});

return app.Run(args);
