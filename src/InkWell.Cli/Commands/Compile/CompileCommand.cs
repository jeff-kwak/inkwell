using System.ComponentModel;
using InkWell.Cli.Tools;
using Spectre.Console;
using Spectre.Console.Cli;

namespace InkWell.Cli.Commands.Compile
{
    public class CompileCommand(
        ITemplateLoader template,
        IContentProcessor content,
        ITemplateRenderer renderer,
        IFileTool file,
        IDirectoryTool directory) : AsyncCommand<CompileCommand.Settings>
    {
        public class Settings() : CommandSettings
        {
            [Description("The source containing an InkWell content and html directory")]
            [CommandArgument(0, "<sourcePath>")]
            public required string SourcePath { get; set; }

            [CommandArgument(1, "<outputPath>")]
            [Description("The output directory for compiled content")]
            public required string OutputPath { get; set; }
        }

        public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
        {
            var source = settings.SourcePath;
            var output = settings.OutputPath;

            // TODO: Validate the source and output paths

            // Clean the output directory
            // This will re-compile everything every time
            directory.CleanDirectory(output);

            // Load the HTML templates
            var templates = await template.LoadTemplates(source);

            // Process content items as they come from the async enumerable
            await foreach (var contentItem in content.Process(source))
            {
                // Get the appropriate template for this content family
                var templateKey = contentItem.Family.ToLowerInvariant();
                if (!templates.TryGetValue(templateKey, out var templateContent))
                {
                    // Skip if no template found for this family
                    AnsiConsole.MarkupLine($"[yellow]No template found for family '{contentItem.Family}'. Skipping content item '{contentItem.Info.Title}'.[/]");
                    continue;
                }

                // Render the template
                var compiled = await renderer.RenderAsync(templateContent, contentItem);

                // Determine the output directory and filename
                var outputDir = contentItem.IsRoot()
                    ? Path.Combine(output)
                    : Path.Combine(
                            output,
                            contentItem.Path.DirName().RemovePathBefore(contentItem.Family));

                var outputFilename = contentItem.Info.FileNameMatchesTitle ?? true ?
                    $"{contentItem.Info.Title.ToSafeFileName()}.html" :
                    $"{contentItem.Path.FileNameWithoutExtension()}.html";

                // Write the rendered HTML to file
                await file.WriteAllTextAsync(compiled, Path.Combine(outputDir, outputFilename));

                // Copy contents of posts and pages that aren't markdown files
                var nonMarkdownFiles = directory.GetFiles(contentItem.Path.DirName()).Where(f => !f.EndsWith(".md", StringComparison.OrdinalIgnoreCase));
                foreach (var nonMarkdownFile in nonMarkdownFiles)
                {
                    // Copy the file to the output directory
                    var destFile = Path.Combine(outputDir, Path.GetFileName(nonMarkdownFile));
                    file.Copy(nonMarkdownFile, destFile);
                }
            }

            // Copy the public directory
            directory.EnsureDirectory(Path.Combine(output, "public"));
            directory.CopyDirectory(Path.Combine(source, "html", "public"), Path.Combine(output, "public"));

            // Copy all the root files except for the index.html file (as it's
            // handled as content))
            var sourceRoot = Path.Combine(source, "html");
            var rootFiles = directory.GetFiles(sourceRoot, "*.*", SearchOption.TopDirectoryOnly)
                .Where(f => !f.EndsWith("index.html", StringComparison.OrdinalIgnoreCase));

            foreach (var rootFile in rootFiles)
            {
                var destFile = Path.Combine(output, Path.GetFileName(rootFile));
                file.Copy(rootFile, destFile);
            }

            return 0;
        }
    }
}
