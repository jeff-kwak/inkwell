using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace InkWell.Cli.Commands
{
    public class CompileCommand : Command<CompileCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [Description("The source containing an InkWell content and html directory")]
            [CommandArgument(0, "<sourcePath>")]
            public required string SourcePath { get; set; }

            [CommandArgument(1, "<outputPath>")]
            [Description("The output directory for compiled content")]
            public required string OutputPath { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            var sourcePath = settings.SourcePath;
            var outputPath = settings.OutputPath;
            AnsiConsole.MarkupLine($"[bold green]Compiling[/] content from [blue]{sourcePath}[/] to [blue]{outputPath}[/]");


            // TODO: Implement the actual compilation logic here
            // This would involve:
            // 1. Reading content files from contentPath
            // 2. Processing (e.g., converting markdown to HTML)
            // 3. Applying templates
            // 4. Writing output to outputPath

            AnsiConsole.MarkupLine("[bold green]Compilation complete![/]");

            return 0;
        }
    }
}
