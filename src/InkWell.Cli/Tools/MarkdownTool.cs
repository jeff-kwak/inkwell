using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;

namespace InkWell.Cli.Tools;

public interface IMarkdownTool
{
    MarkdownDocument Parse(string markdown);
    string ToHtml(string markdown);
}

public class MarkdownTool : IMarkdownTool
{
    // TODO: expose this for configuration
    private static MarkdownPipeline Pipeline =
        new MarkdownPipelineBuilder()
        .UseYamlFrontMatter()
        .UseAdvancedExtensions()
        .Build();

    public MarkdownDocument Parse(string markdown)
    {
        return Markdown.Parse(markdown, Pipeline);
    }

    public string ToHtml(string markdown)
    {
        return Markdown.ToHtml(markdown, Pipeline);
    }
}
