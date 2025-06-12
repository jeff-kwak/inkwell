using Stubble.Core;
using Stubble.Core.Builders;

namespace InkWell.Cli.Commands.Compile;

public interface ITemplateRenderer
{
    Task<string> RenderAsync(string template, MarkdownContent content);
}

public class TemplateRenderer : ITemplateRenderer
{
    private static readonly StubbleVisitorRenderer render =
        new StubbleBuilder().Build();

    public async Task<string> RenderAsync(string template, MarkdownContent content)
    {
        var state = TemplateData.FromMarkdownContent(content);
        return await render.RenderAsync(template, state);
    }
}
