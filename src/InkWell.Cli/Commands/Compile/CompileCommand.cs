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

                // // TODO: Don't think that genie figured this out right either but
                // // it's close.
                // // Generate output file path
                // // For root content, use index.html, otherwise use family/title structure
                // string outputFilePath;
                // if (contentItem.Family.Equals("root", StringComparison.OrdinalIgnoreCase))
                // {
                //     outputFilePath = Path.Combine(output, "index.html");
                // }
                // else
                // {
                // Create a safe filename from the title
                var safeTitle = string.Join("", contentItem.Info.Title.ToLowerInvariant()
                    .Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-'))
                    .Replace(' ', '-');
                var outputFilePath = Path.Combine(output, contentItem.Family, $"{safeTitle}.html");
                // }

                // Write the rendered HTML to file
                await file.WriteAllTextAsync(compiled, outputFilePath);
            }

            return 0;
        }
    }
}
