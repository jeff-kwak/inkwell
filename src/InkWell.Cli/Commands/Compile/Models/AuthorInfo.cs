namespace InkWell.Cli.Commands.Compile.Models;

public record AuthorInfo(string Name, string Email)
{
    public AuthorInfo() : this(
        string.Empty,
        string.Empty)
    { } // for YamlDotNet
}
