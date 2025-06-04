using InkWell.Cli.Commands.Compile;
using InkWell.Cli.Tools;
using NSubstitute;

namespace InkWell.FastTests.Commands.Compile;

public class ContentProcessorTests
{
    private string ContentWithFrontMatter =>
        """
        ---
        title: Test Content
        ---

        # Test Content

        This is a test content with front-matter.
        """;

    private string ContentWithoutFrontMatter =>
        """
        # Test Content

        This is a test content without front-matter.
        """;

    [Test]
    public async Task Process_FindsContentInFamilies()
    {
        const string source = "/test/source";

        var directory = Substitute.For<IDirectoryTool>();
        directory.GetDirectories(Arg.Is(source.Path("content")), Arg.Is("*"), SearchOption.TopDirectoryOnly)
            .Returns([source.Path("content/family1"), source.Path("content/family2")]);
        directory.GetFiles(Arg.Is(source.Path("content/family1")), Arg.Any<string>(), SearchOption.AllDirectories).
            Returns([source.Path("content/family1/content1.md")]);
        directory.GetFiles(Arg.Is(source.Path("content/family2")), Arg.Any<string>(), SearchOption.AllDirectories).
            Returns([source.Path("content/family2/content2.md")]);
        directory.GetFiles(Arg.Is(source.Path("content")), Arg.Any<string>(), SearchOption.AllDirectories).Returns([
            source.Path("content/index.md"),
        ]);

        var file = Substitute.For<IFileTool>();
        file.ReadAllTextAsync(Arg.Any<string>()).Returns(ContentWithFrontMatter);

        var processor = new ContentProcessor(directory, file);

        var result = await processor.Process(source).ToListAsync();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Info.Title, Is.EqualTo("Test Content"));
            Assert.That(result[0].Family, Is.EqualTo("root"));
            Assert.That(result[0].Html, Is.Not.Null.Or.Empty);

            Assert.That(result[1].Info.Title, Is.EqualTo("Test Content"));
            Assert.That(result[1].Family, Is.EqualTo("family1"));
            Assert.That(result[1].Html, Is.Not.Null.Or.Empty);

            Assert.That(result[2].Info.Title, Is.EqualTo("Test Content"));
            Assert.That(result[2].Family, Is.EqualTo("family2"));
            Assert.That(result[2].Html, Is.Not.Null.Or.Empty);
        });
    }

}
