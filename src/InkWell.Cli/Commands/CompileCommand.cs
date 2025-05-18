using InkWell.Cli.Tools;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using InkWell.Cli.Boilerplate;

namespace InkWell.Cli.Commands
{
    public class CompileCommand(IDirectoryTool directory) : Command<CompileCommand.Settings>
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

            // Verify that the source path exists and looks like an InkWell directory
            AnsiConsole.MarkupLine($"[bold green]Compiling[/] content from [blue]{sourcePath}[/] to [blue]{outputPath}[/]");

            if (!IsInkWellSourceDirectory(sourcePath))
            {
                return 1;
            }

            // Create the destination directory if it doesn't exist
            EnsureOutputDirectory(outputPath);

            // Remove the contents of the output directory
            CleanOutputDirectory(outputPath);

            // Copy the public directory to the output directory
            string publicPath = Path.Combine(sourcePath, "html/public");
            directory.CopyDirectory(publicPath, Path.Combine(outputPath, "public"), true);

            AnsiConsole.MarkupLine($"[bold green]Copying[/] public directory [blue]{publicPath}[/] to [blue]{outputPath}[/]");

            // Process the front matter
            AnsiConsole.MarkupLine("[bold green]Compilation complete![/]");

            return 0;
        }

        private void CleanOutputDirectory(string outputPath)
        {
            AnsiConsole.MarkupLine($"[bold green]Cleaning[/] output directory [blue]{outputPath}[/]");
            var files = directory.GetFiles(outputPath, "*", SearchOption.AllDirectories);
            files.ForEach(file => directory.DeleteFile(file));
            var directories = directory.GetDirectories(outputPath, "*", SearchOption.AllDirectories);
            directories.ForEach(dir => directory.DeleteDirectory(dir));
        }

        private void EnsureOutputDirectory(string outputPath)
        {
            AnsiConsole.MarkupLine($"[bold green]Ensuring[/] output directory [blue]{outputPath}[/] exists");
            if (!directory.Exists(outputPath))
            {
                directory.CreateDirectory(outputPath);
            }
        }

        private bool IsInkWellSourceDirectory(string path)
        {
            if (!directory.Exists(path))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] Source path [blue]{path}[/] does not exist. Nothing to compile.");
                return false;
            }

            // Verify that content and html directories exist
            string contentPath = Path.Combine(path, "content");
            string htmlPath = Path.Combine(path, "html");

            if (!directory.Exists(contentPath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] InkWell source directories need to have a directory called 'content'. Content directory [blue]{contentPath}[/] not found in source path.");
                return false;
            }

            if (!directory.Exists(htmlPath))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] InkWell source directories need to have a directory called 'html' directory [blue]{htmlPath}[/] not found in source path.");
                return false;
            }

            return true;
        }
    }
}
