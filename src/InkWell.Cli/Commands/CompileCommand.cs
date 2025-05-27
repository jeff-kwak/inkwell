using Spectre.Console.Cli;
using System.ComponentModel;

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

            // TODO: Validate the source path

            // 1. Load the HTML templates
            //  Every top-level directory is a  "family". The top most
            //    directory is "root". The data for the whole site is available
            //    to all templates under the "root" settings.
            // 2. For each markdown file:
            //    - Parse the markdown file for YAML and add to context.
            //    - Convert the markdown to HTML
            //    - Render the HTML using the template for the family
            // 3. Copy the static HTML files (favicon and public/)

            return 0;
        }
    }
}
