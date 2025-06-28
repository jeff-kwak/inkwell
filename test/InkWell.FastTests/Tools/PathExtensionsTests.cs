using InkWell.Cli.Tools;
using NUnit.Framework;

namespace InkWell.FastTests.Tools;

public class PathExtensionsTests
{
    [Test]
    public void Path_WithSinglePath_ReturnsCombinedPath()
    {
        var basePath = "/home/user";
        var result = basePath.Path("documents");

        Assert.That(result, Is.EqualTo("/home/user/documents"));
    }

    [Test]
    public void Path_WithMultiplePaths_CombinesAllPaths()
    {
        var basePath = "/home/user";
        var result = basePath.Path("documents", "projects", "inkwell");

        Assert.That(result, Is.EqualTo("/home/user/documents/projects/inkwell"));
    }

    [Test]
    public void Path_WithEmptyBasePath_CombinesCorrectly()
    {
        var basePath = "";
        var result = basePath.Path("documents", "file.txt");

        Assert.That(result, Is.EqualTo("documents/file.txt"));
    }

    [Test]
    public void ToSafeFileName_WithBasicTitle_ReturnsLowercaseWithDashes()
    {
        var title = "Hello World";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world"));
    }

    [Test]
    public void ToSafeFileName_WithSpecialCharacters_ReplacesWithSingleDash()
    {
        var title = "Hello, World! & More";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world-more"));
    }

    [Test]
    public void ToSafeFileName_WithConsecutiveSpecialChars_ProducesSingleDash()
    {
        var title = "Hello!!!World???Test";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world-test"));
    }

    [Test]
    public void ToSafeFileName_WithSpacesAndSpecialChars_ProducesSingleDash()
    {
        var title = "Hello   !!!   World";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world"));
    }

    [Test]
    public void ToSafeFileName_WithLeadingAndTrailingSpecialChars_TrimsLeadingTrailingDashes()
    {
        var title = "!!!Hello World!!!";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world"));
    }

    [Test]
    public void ToSafeFileName_WithOnlySpecialCharacters_ReturnsEmptyString()
    {
        var title = "!@#$%^&*()";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public void ToSafeFileName_WithNumbers_PreservesNumbers()
    {
        var title = "Article 123 Version 2.0";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("article-123-version-2.0"));
    }

    [Test]
    public void ToSafeFileName_WithMixedCase_ConvertsToLowercase()
    {
        var title = "My AWESOME Blog Post";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("my-awesome-blog-post"));
    }

    [Test]
    public void ToSafeFileName_AcceptsUnicodeCharacters()
    {
        var title = "Café & Résumé";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("café-résumé"));
    }

    [Test]
    public void ToSafeFileName_WithEmptyString_ReturnsEmptyString()
    {
        var title = "";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public void ToSafeFileName_WithOnlySpaces_ReturnsEmptyString()
    {
        var title = "   ";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo(""));
    }

    [Test]
    public void ToSafeFileName_WithExistingDashes_PreservesSingleDashes()
    {
        var title = "hello-world-test";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world-test"));
    }

    [Test]
    public void ToSafeFileName_WithMixedDashesAndSpaces_ProducesSingleDashes()
    {
        var title = "hello - world - test";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("hello-world-test"));
    }

    [Test]
    public void ToSafeFileName_WithComplexTitle_HandlesCorrectly()
    {
        var title = "How to: Build & Deploy C# Apps (Part 1/3)";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("how-to-build-deploy-c-apps-part-1-3"));
    }

    [Test]
    public void ToSafeFileName_WithFileExtensionLikePattern_PreservesDots()
    {
        var title = "my.file.name.txt";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("my.file.name.txt"));
    }

    [Test]
    public void ToSafeFileName_WithPathSeparators_ReplacesWithDashes()
    {
        var title = "folder/subfolder\\filename";
        var result = title.ToSafeFileName();

        Assert.That(result, Is.EqualTo("folder-subfolder-filename"));
    }

    [Test]
    public void GetDirectoryName_WithValidPath_ReturnsDirectoryName()
    {
        var path = "/home/user/documents/file.txt";
        var result = path.DirName();

        Assert.That(result, Is.EqualTo("/home/user/documents"));
    }

    [Test]
    public void RemovePathBefore_WithFamilyInPath_ReturnsPathAfterFamily()
    {
        var path = "/home/user/documents/inkwell/content/item.md";
        var family = "inkwell";
        var result = path.RemovePathBefore(family);

        Assert.That(result, Is.EqualTo("inkwell/content/item.md"));
    }

    [Test]
    public void RemovePathBefore_WithFamilyNotInPath_ReturnsOriginalPath()
    {
        var path = "/home/user/documents/inkwell/content/item.md";
        var family = "unknown";
        var result = path.RemovePathBefore(family); // Family not found in path

        Assert.That(result, Is.EqualTo(path)); // Should return the original path
    }
}
