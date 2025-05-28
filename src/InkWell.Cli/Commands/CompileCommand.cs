using System.ComponentModel;
using InkWell.Cli.Tools;
using Spectre.Console.Cli;
using static System.Console;

namespace InkWell.Cli.Commands
{
    public class CompileCommand(IFileTool file, IDirectoryTool directory) : AsyncCommand<CompileCommand.Settings>
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
            //    - The templates are in the html/templates directory.
            //    - The templates are named after the family they belong to.
            //    - The root template is named "index.html" and is used for the root family.
            // 2. Load the content files
            //  Every top-level directory is a  "family". The top most
            //    directory is "root". The data for the whole site is available
            //    to all templates under the "root" settings.
            // 2. For each markdown file:
            //    - Parse the markdown file for YAML and add to context.
            //    - Convert the markdown to HTML
            //    - Render the HTML using the template for the family
            // 3. Copy the static HTML files (favicon and public/)

            var htmlDir = source.Path("html");

            Dictionary<string, string> templates = [];
            templates["root"] = await file.ReadAllTextAsync(htmlDir, "index.html");

            var templateDir = htmlDir.Path("templates");

            var htmlTemplates = directory.GetFiles(templateDir, "*.html");

            foreach (var template in htmlTemplates)
            {
                var family = Path.GetFileNameWithoutExtension(template).ToLowerInvariant();
                WriteLine($"Loading template: {family}");
                templates[family] = await file.ReadAllTextAsync(template);
            }

            return 0;
        }
    }
}
