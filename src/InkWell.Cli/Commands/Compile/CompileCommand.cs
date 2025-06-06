using System.ComponentModel;
using InkWell.Cli.Tools;
using Spectre.Console;
using Spectre.Console.Cli;
using Stubble.Core.Builders;

namespace InkWell.Cli.Commands.Compile
{
    public class CompileCommand(ITemplateLoader template, IContentProcessor content, IFileTool file) : AsyncCommand<CompileCommand.Settings>
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

            // TODO: Validate the source path

            // 1. Load the HTML templates
            var templates = await template.LoadTemplates(source);

            // 2. Process content files and render HTML
            var stubble = new StubbleBuilder().Build();

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

                // Render the template with Stubble
                var renderedHtml = await stubble.RenderAsync(templateContent, contentItem);

                // Generate output file path
                // For root content, use index.html, otherwise use family/title structure
                string outputFilePath;
                if (contentItem.Family.Equals("root", StringComparison.OrdinalIgnoreCase))
                {
                    outputFilePath = Path.Combine(output, "index.html");
                }
                else
                {
                    // Create a safe filename from the title
                    var safeTitle = string.Join("", contentItem.Info.Title.ToLowerInvariant()
                        .Where(c => char.IsLetterOrDigit(c) || c == ' ' || c == '-'))
                        .Replace(' ', '-');
                    outputFilePath = Path.Combine(output, contentItem.Family, $"{safeTitle}.html");
                }

                // Write the rendered HTML to file
                await file.WriteAllTextAsync(renderedHtml, outputFilePath);
            }

            return 0;
        }
    }
}
