using InkWell.Cli.Commands.Compile.Models;
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
        .Register("Year", (HelperContext ctx, DateTime date) => date.Year.ToString("0000"));

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
