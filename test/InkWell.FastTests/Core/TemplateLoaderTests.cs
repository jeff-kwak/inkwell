using InkWell.Cli.Core;
using InkWell.Cli.Tools;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace InkWell.FastTests.Core;

public class TemplateLoaderTests
{
    private IDirectoryTool directory;
    private IFileTool file;

    [SetUp]
    public void SetUp()
    {
        directory = Substitute.For<IDirectoryTool>();
        file = Substitute.For<IFileTool>();
    }

    [Test]
    public async Task LoadTemplates_WhenCalled_LoadsRootTemplate()
    {
        const string sourcePath = "/test/source";
        const string expectedHtmlDir = "/test/source/html";
        const string rootTemplateContent = "<html><body>Root Template</body></html>";

        file.ReadAllTextAsync(expectedHtmlDir, "index.html")
            .Returns(rootTemplateContent);

        directory.GetFiles(Arg.Any<string>(), Arg.Any<string>())
            .Returns([]);

        var loader = new TemplateLoader(directory, file);

        var result = await loader.LoadTemplates(sourcePath);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.ContainsKey("root"), Is.True);
            Assert.That(result["root"], Is.EqualTo(rootTemplateContent));
        });

    }


    [Test]
    public async Task LoadTemplates_WithMultipleTemplateFiles_LoadsAllTemplates()
    {
        // Arrange
        const string sourcePath = "/test/source";
        var templateFiles = new[]
        {
            "/test/source/html/templates/pages.html",
            "/test/source/html/templates/posts.html",
            "/test/source/html/templates/archives.html"
        };

        const string rootContent = "<html>Root</html>";
        const string pagesContent = "<html>Pages Template</html>";
        const string postsContent = "<html>Posts Template</html>";
        const string archivesContent = "<html>Archives Template</html>";

        file.ReadAllTextAsync(Arg.Is<string>(s => s.EndsWith("html")), "index.html")
            .Returns(rootContent);

        file.ReadAllTextAsync("/test/source/html/templates/pages.html")
            .Returns(pagesContent);

        file.ReadAllTextAsync("/test/source/html/templates/posts.html")
            .Returns(postsContent);

        file.ReadAllTextAsync("/test/source/html/templates/archives.html")
            .Returns(archivesContent);

        directory.GetFiles(Arg.Any<string>(), "*.html")
            .Returns(templateFiles);

        var loader = new TemplateLoader(directory, file);

        var result = await loader.LoadTemplates(sourcePath);

        Assert.That(result, Has.Count.EqualTo(4));
        Assert.Multiple(() =>
        {
            Assert.That(result["root"], Is.EqualTo(rootContent));
            Assert.That(result["pages"], Is.EqualTo(pagesContent));
            Assert.That(result["posts"], Is.EqualTo(postsContent));
            Assert.That(result["archives"], Is.EqualTo(archivesContent));
        });
    }


    [Test]
    public async Task LoadTemplates_WithTemplateFiles_UsesFileNameWithoutExtensionAsKey()
    {
        const string sourcePath = "/test/source";
        var templateFiles = new[]
        {
            "/path/to/MySpecialTemplate.html",
            "/path/to/UPPERCASE.html"
        };

        file.ReadAllTextAsync(Arg.Any<string>(), "index.html")
            .Returns("<html>Root</html>");

        file.ReadAllTextAsync("/path/to/MySpecialTemplate.html")
            .Returns("<html>Special</html>");

        file.ReadAllTextAsync("/path/to/UPPERCASE.html")
            .Returns("<html>Upper</html>");

        directory.GetFiles(Arg.Any<string>(), "*.html")
            .Returns(templateFiles);

        var loader = new TemplateLoader(directory, file);

        var result = await loader.LoadTemplates(sourcePath);

        // Assert
        Assert.That(result.ContainsKey("myspecialtemplate"), Is.True);
        Assert.That(result.ContainsKey("uppercase"), Is.True);
        Assert.That(result["myspecialtemplate"], Is.EqualTo("<html>Special</html>"));
        Assert.That(result["uppercase"], Is.EqualTo("<html>Upper</html>"));
    }


    [Test]
    public async Task LoadTemplates_WithNoTemplateFiles_ReturnsOnlyRootTemplate()
    {
        const string sourcePath = "/test/source";
        const string rootContent = "<html>Root Only</html>";

        file.ReadAllTextAsync(Arg.Any<string>(), "index.html")
            .Returns(rootContent);

        directory.GetFiles(Arg.Any<string>(), "*.html")
            .Returns(Array.Empty<string>());

        var loader = new TemplateLoader(directory, file);

        var result = await loader.LoadTemplates(sourcePath);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result.ContainsKey("root"), Is.True);
            Assert.That(result["root"], Is.EqualTo(rootContent));
        });

    }


    [Test]
    public async Task LoadTemplates_WhenFileToolThrows_PropagatesException()
    {
        const string sourcePath = "/test/source";
        var expectedException = new FileNotFoundException("Template file not found");

        file.ReadAllTextAsync(Arg.Any<string>(), "index.html").Throws(expectedException);

        var loader = new TemplateLoader(directory, file);

        await Assert.ThatAsync(() => loader.LoadTemplates(sourcePath), Throws.Exception
            .TypeOf<FileNotFoundException>()
            .With.Message.EqualTo("Template file not found"));
    }

    [Test]
    public async Task LoadTemplates_WhenDirectoryToolThrows_PropagatesException()
    {
        // Arrange
        const string sourcePath = "/test/source";
        var expectedException = new DirectoryNotFoundException("Templates directory not found");

        file.ReadAllTextAsync(Arg.Any<string>(), "index.html")
            .Returns("<html>Root</html>");

        directory.GetFiles(Arg.Any<string>(), "*.html")
            .Throws(expectedException);

        var loader = new TemplateLoader(directory, file);

        await Assert.ThatAsync(() => loader.LoadTemplates(sourcePath), Throws.Exception
            .TypeOf<DirectoryNotFoundException>()
            .With.Message.EqualTo("Templates directory not found"));
    }

    [Test]
    public async Task LoadTemplates_WithTemplateReadFailure_PropagatesException()
    {
        // Arrange
        const string sourcePath = "/test/source";
        var templateFiles = new[] { "/test/template.html" };
        var expectedException = new UnauthorizedAccessException("Access denied");

        file.ReadAllTextAsync(Arg.Is<string>(s => s.EndsWith("html")), "index.html")
            .Returns("<html>Root</html>");

        file.ReadAllTextAsync("/test/template.html")
            .Throws(expectedException);

        directory.GetFiles(Arg.Any<string>(), "*.html")
            .Returns(templateFiles);

        var loader = new TemplateLoader(directory, file);


        await Assert.ThatAsync(() => loader.LoadTemplates(sourcePath), Throws.Exception
            .TypeOf<UnauthorizedAccessException>()
            .With.Message.EqualTo("Access denied"));
    }
}
