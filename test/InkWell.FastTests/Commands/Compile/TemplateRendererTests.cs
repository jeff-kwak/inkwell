using InkWell.Cli.Commands.Compile;

namespace InkWell.FastTests.Commands.Compile;

public class TemplateRendererTests
{
    private static readonly MarkdownContent Data = new(
        Info: new FrontMatter(
            Title: "Test Title",
            Desc: "Test Description",
            Updated: new DateTime(2025, 06, 22, 9, 31, 0),
            Created: new DateTime(2025, 06, 22, 9, 30, 0),
            Published: new DateTime(2025, 06, 22, 9, 30, 0),
            Summary: "Test Summary",
            Author: new AuthorInfo("John Doe", "john.doe@example.com")
        ),
        Family: "Test Family",
        Html: "<p>Test HTML content</p>"
    );

    [Test]
    public async Task RenderTemplate_TitleShouldBeReplaced()
    {
        var template = "{{Title}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("Test Title"));
    }

    [Test]
    public async Task RenderTemplate_CaseInsensitiveTitleShouldBeReplaced()
    {
        var template = "{{title}}"; // lowercase
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("Test Title"));
    }

    [Test]
    public async Task RenderTemplate_DescShouldBeReplaced()
    {
        var template = "{{desc}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("Test Description"));
    }

    [Test]
    public async Task RenderTemplate_NullDescriptionShouldBeEmpty()
    {
        var template = "{{DESC}}";
        var missingDescription = Data with { Info = Data.Info with { Desc = null } };
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, missingDescription);

        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public async Task RenderTemplate_UpdatedDateShouldBeRenderedAsISO8601()
    {
        var template = "{{Updated}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("2025-06-22T09:31:00"));
    }

    [Test]
    public async Task RenderTemplate_UpdatedDateWithNullShouldBeEmpty()
    {
        var template = "{{updated}}";
        var missingUpdated = Data with { Info = Data.Info with { Updated = null } };
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, missingUpdated);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task RenderTemplate_PublishedDateShouldBeRenderedAsISO8601()
    {
        var template = "{{Published}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("2025-06-22T09:30:00"));
    }

    [Test]
    public async Task RenderTemplate_PublishedDateWithNullShouldBeEmpty()
    {
        var template = "{{published}}";
        var missingPublished = Data with { Info = Data.Info with { Published = null } };
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, missingPublished);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task RenderTemplate_CreatedDateShouldBeRenderedAsISO8601()
    {
        var template = "{{Created}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("2025-06-22T09:30:00"));
    }

    [Test]
    public async Task RenderTemplate_CreatedDateWithNullShouldBeEmpty()
    {
        var template = "{{created}}";
        var missingCreated = Data with { Info = Data.Info with { Created = null } };
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, missingCreated);

        Assert.That(result, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task RenderTemplate_SummaryShouldBeReplaced()
    {
        var template = "{{Summary}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("Test Summary"));
    }

    [Test]
    public async Task RenderTemplate_AuthorNameShouldBeReplaced()
    {
        var template = "{{Author.Name}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("John Doe"));
    }

    [Test]
    public async Task RenderTemplate_AuthorEmailShouldBeReplaced()
    {
        var template = "{{Author.Email}}";
        var renderer = new TemplateRenderer();

        var result = await renderer.RenderAsync(template, Data);

        Assert.That(result, Is.EqualTo("john.doe@example.com"));
    }
}
