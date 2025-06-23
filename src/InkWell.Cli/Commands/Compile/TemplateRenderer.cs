using Stubble.Core;
using Stubble.Core.Builders;
using Stubble.Helpers;

namespace InkWell.Cli.Commands.Compile;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(string template, MarkdownContent content);
}

public class TemplateRenderer : ITemplateRenderer
{
    private static readonly Helpers helpers =
        new Helpers()
        .Register("Year", (HelperContext ctx, DateTime date) => date.Year.ToString("0000"))
        .Register("Date", (HelperContext ctx, string Format, string CultureInfo, DateTime date) =>
        {
            if (string.IsNullOrEmpty(CultureInfo))
            {
                CultureInfo = "en-US"; // Default to en-US if no culture is specified
            }

            if (string.IsNullOrEmpty(Format))
            {
                return date.ToString("yyyy-MM-dd");
            }

            return date.ToString(Format, new System.Globalization.CultureInfo(CultureInfo));

        });

    private static readonly StubbleVisitorRenderer render =
        new StubbleBuilder()
        .Configure(cfg =>
        {
            cfg.AddHelpers(helpers);
            cfg.SetIgnoreCaseOnKeyLookup(true);
        })
        .Build();


    public async Task<string> RenderAsync(string template, MarkdownContent content)
    {
        var state = TemplateData.FromMarkdownContent(content);
        return await render.RenderAsync(template, state);
    }
}
