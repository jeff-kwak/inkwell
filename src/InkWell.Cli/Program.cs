using Spectre.Console.Cli;
using InkWell.Cli.Commands;

var app = new CommandApp();

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
