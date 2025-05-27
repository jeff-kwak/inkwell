using System.ComponentModel;
using InkWell.Cli.Boilerplate;
using InkWell.Cli.Tools;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using Spectre.Console;
using Spectre.Console.Cli;

namespace InkWell.Cli.Commands
{
    public class CompileCommand(
        IDirectoryTool directory,
        IFileTool file,
        IMarkdownTool markdown,
        IYamlTool yaml,
        IMustacheTool mustache)
        : Command<CompileCommand.Settings>
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
            AnsiConsole.MarkupLine($"[bold green]Copying[/] public directory [blue]{publicPath}[/] to [blue]{outputPath}[/]");
            directory.CopyDirectory(publicPath, Path.Combine(outputPath, "public"), true);

            // First process any root markdown files.
            var rootFiles = directory.GetFiles(sourcePath, "*.md", SearchOption.TopDirectoryOnly) .ToList();
            foreach (var rootFile in rootFiles)
            {
                AnsiConsole.MarkupLine($"[bold green]Processing[/] root markdown file [blue]{rootFile}[/]");
                var (data, mustache) = ProcessMarkdown(rootFile, "root");
                if (data != null)
                {
                    // Save the processed data to the output directory
                    var outputFilePath = Path.Combine(outputPath, "content", data.Path);
                    directory.CreateDirectory(Path.GetDirectoryName(outputFilePath)!);
                    file.WriteAllText(outputFilePath, mustache);
                }
            }


            AnsiConsole.MarkupLine("[bold green]Compilation complete![/]");
            return 0;
        }

        private (ContentData? data, string mustache) ProcessMarkdown(string sourcePath, string collection)
        {
            var data = new Dictionary<string, object>();
            var mustache = string.Empty;


            var fileContent = file.ReadAllText(sourcePath);
            var doc = markdown.Parse(fileContent);
            var frontMatterContent = doc.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

            if (frontMatterContent == null)
            {
                AnsiConsole.MarkupLine($"[bold yellow]Error:[/] No front matter found in [blue]{sourcePath}[/]. Skipping file.");
                return (null, string.Empty);
            }

            var frontMatter = yaml.Deserialize(frontMatterContent);
            return (
                new ContentData(
                    Collection: collection,
                    Path: sourcePath,
                    Title: frontMatter["title"]?.ToString() ?? string.Empty,
                    Description: frontMatter["description"]?.ToString() ?? string.Empty,
                    Author: new Author(
                        Name: frontMatter["author"] is Dictionary<string, object> authorDict2 ? authorDict2["name"]?.ToString() ?? string.Empty : string.Empty,
                        Email: frontMatter["author"] is Dictionary<string, object> authorDict ? authorDict["email"]?.ToString() : null
                    ),
                    CreatedAt: DateTime.Parse(frontMatter["createdAt"]?.ToString() ?? DateTime.MinValue.ToString()),
                    UpdatedAt: DateTime.Parse(frontMatter["updatedAt"]?.ToString() ?? DateTime.MinValue.ToString()),
                    PublishedAt: DateTime.Parse(frontMatter["publishedAt"]?.ToString() ?? DateTime.MinValue.ToString()),
                    IsDraft: bool.Parse(frontMatter["isDraft"]?.ToString() ??  "false"),
                    Tags: frontMatter["tags"] != null
                        ? [.. frontMatter["tags"]!.ToString()!.Split(',').Select(tag => tag.Trim())]
                        : []
                ),
                doc.ToHtml()
            );
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

            // Verify that the index.md file exists
            if (!file.Exists(Path.Combine(path, "index.md")))
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] InkWell source directories need to have an index.md file. Use the index.md file to hold site-level metadata in the front matter. [blue]{Path.Combine(path, "index.md")}[/] not found in source path.");
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
