using InkWell.Cli.Commands;
using InkWell.Cli.Tools;
using NSubstitute;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Testing;

namespace InkWell.FastTests.Commands;

public class CompileCommandTests
{
    private TestConsole console = null!;

    [SetUp]
    public void BeforeEach()
    {
        // Create the console for capturing output
        console = new TestConsole();
        AnsiConsole.Console = console;
    }

    [TearDown]
    public void AfterEach()
    {
        console?.Dispose();
    }

    [Test]
    public void Execute_WithNonExistentSourcePath_ReturnsError()
    {
        var directory = Substitute.For<IDirectoryTool>();
        directory.Exists(Arg.Any<string>()).Returns(false);
        var remaining = Substitute.For<IRemainingArguments>();
        var context = new CommandContext([], remaining, "test", null);
        var command = new CompileCommand(directory);

        int result = command.Execute(context, new CompileCommand.Settings
        {
            SourcePath = "nonexistent/path",
            OutputPath = "output/path"
        });

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(1), "Expected command to return error code 1");
            Assert.That(console.Output, Contains.Substring("Error: Source path"), "Expected error message about non-existent source path");
        });

        Assert.That(console.Output, Does.Contain("does not exist"), "Expected 'does not exist' in error message");
    }

    [Test]
    public void Execute_WithMissingContentDirectory_ReturnsError()
    {
        var directory = Substitute.For<IDirectoryTool>();
        directory.Exists(Arg.Is("source/path")).Returns(true);
        directory.Exists(Arg.Is("source/path/content")).Returns(false);
        var remaining = Substitute.For<IRemainingArguments>();
        var context = new CommandContext([], remaining, "test", null);
        var command = new CompileCommand(directory);

        int result = command.Execute(context, new CompileCommand.Settings
        {
            SourcePath = "source/path",
            OutputPath = "output/path"
        });

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(1), "Expected command to return error code 1");
            Assert.That(console.Output, Contains.Substring("Error: InkWell source directories need to have a directory called 'content'"), "Expected error message about missing content directory");
        });

        Assert.That(console.Output, Does.Contain("not found in source path"), "Expected 'not found in source path' in error message");
    }

    [Test]
    public void Execute_WithMissingHtmlDirectory_ReturnsError()
    {
        var directory = Substitute.For<IDirectoryTool>();
        directory.Exists(Arg.Is("source/path")).Returns(true);
        directory.Exists(Arg.Is("source/path/content")).Returns(true);
        directory.Exists(Arg.Is("source/path/html")).Returns(false);
        var remaining = Substitute.For<IRemainingArguments>();
        var context = new CommandContext([], remaining, "test", null);
        var command = new CompileCommand(directory);

        int result = command.Execute(context, new CompileCommand.Settings
        {
            SourcePath = "source/path",
            OutputPath = "output/path"
        });

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(1), "Expected command to return error code 1");
            Assert.That(console.Output, Contains.Substring("Error: InkWell source directories need to have a directory called 'html'"), "Expected error message about missing HTML directory");
        });

        Assert.That(console.Output, Does.Contain("not found in source path"), "Expected 'not found in source path' in error message");
    }

    [Test]
    public void Execute_WithValidDirectoryStructure_ReturnsSuccess()
    {
        var directory = Substitute.For<IDirectoryTool>();
        directory.Exists(Arg.Is("source/path")).Returns(true);
        directory.Exists(Arg.Is("source/path/content")).Returns(true);
        directory.Exists(Arg.Is("source/path/html")).Returns(true);
        var remaining = Substitute.For<IRemainingArguments>();
        var context = new CommandContext([], remaining, "test", null);
        var command = new CompileCommand(directory);

        int result = command.Execute(context, new CompileCommand.Settings
        {
            SourcePath = "source/path",
            OutputPath = "output/path"
        });

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(0), "Expected command to return success code 0");
            Assert.That(console.Output, Contains.Substring("Compiling"), "Expected output to contain 'Compiling'");
            Assert.That(console.Output, Contains.Substring("Compilation complete"), "Expected output to contain 'Compilation complete'");
        });
    }
}
