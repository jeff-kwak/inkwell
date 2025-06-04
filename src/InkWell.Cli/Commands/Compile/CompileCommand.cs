using System.ComponentModel;
using Spectre.Console.Cli;

namespace InkWell.Cli.Commands.Compile
{
    public class CompileCommand(ITemplateLoader template/*, IContentProcessor content*/) : AsyncCommand<CompileCommand.Settings>
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
            //    - The root template is named "index.html" and is used for the
            //      root family.
            // 2. Load the content files
            //  - Every top-level directory is a  "family".
            //  - The top most directory is "root". The data for the whole
            //    site is available to all templates under the "root" settings.
            // 4. Render the HTML using the templates
            // 5. Copy the static HTML files (favicon.ico, all other files
            //    except index.html, and public/)

            var templates = await template.LoadTemplates(source);
            // var content = await content.Process(source);

            return 0;
        }
    }
}
