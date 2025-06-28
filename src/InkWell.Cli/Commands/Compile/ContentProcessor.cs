using InkWell.Cli.Tools;
using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace InkWell.Cli.Commands.Compile;

public interface IContentProcessor
{
    IAsyncEnumerable<MarkdownContent> Process(string sourcePath);
}

public class ContentProcessor(IDirectoryTool directory, IFileTool file) : IContentProcessor
{
    // Doing this locally. Could be moved to the setup if needed.
    private static MarkdownPipeline BuildMarkdownPipeline() =>
        // Create a Markdown pipeline with YAML front-matter support.
        new MarkdownPipelineBuilder()
            .UseYamlFrontMatter()
            .Build();

    private static IDeserializer BuildYamlDeserializer() =>
        // Create a YAML deserializer with camel case naming convention.
        new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

    public async IAsyncEnumerable<MarkdownContent> Process(string sourcePath)
    {
        var markdownPipeline = BuildMarkdownPipeline();
        var yaml = BuildYamlDeserializer();

        // Get all the "family" names. The "families" are the top-level
        // directories in the source/content path. Add the content path which
        // will be used as the root.
        string[] familyDirs = [sourcePath.Path("content"), .. directory.GetDirectories(sourcePath.Path("content"), "*", SearchOption.TopDirectoryOnly)];
        foreach (var familyDir in familyDirs)
        {
            // Get all the markdown files in the family directory.
            var markdownFiles = directory.GetFiles(familyDir, "*.md", SearchOption.AllDirectories);

            foreach (var markdownPath in markdownFiles)
            {
                // Read the content of the markdown file.
                var content = await file.ReadAllTextAsync(markdownPath);

                // Extract the front-matter from the markdown file.
                var document = Markdown.Parse(content, markdownPipeline);
                var frontMatter = document.Descendants<YamlFrontMatterBlock>().FirstOrDefault();
                if (frontMatter is null)
                {
                    continue; // Skip files without front-matter.
                }

                // Convert the frontMatter into a content info object.
                var contentInfo = yaml.Deserialize<FrontMatter>(frontMatter.Lines.ToString());

                // Process the raw markdown content into HTML.
                var html = Markdown.ToHtml(content, markdownPipeline);

                // remove the sourcePath.Path("content") from the markdownPath and get directory only
                var familyPath = (Path.GetDirectoryName(markdownPath) ?? string.Empty)
                    .Replace(sourcePath.Path("content"), string.Empty)
                    .TrimStart(Path.DirectorySeparatorChar);

                // Extract the first directory name as the family name.
                var familyName = familyPath.Split(Path.DirectorySeparatorChar).FirstOrDefault();

                // Create a MarkdownContent object and yield it.
                yield return new MarkdownContent(
                    Info: contentInfo,
                    Path: markdownPath,
                    Family: string.IsNullOrEmpty(familyName) ? "root" : familyName,
                    Html: html
                );
            }
        }
    }
}
