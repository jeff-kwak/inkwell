namespace InkWell.Cli.Commands;

public record Author(string Name, string? Email);

public record ContentData(
    string Collection,
    string Path,
    string Title,
    string Description,
    Author? Author,
    DateTime? CreatedAt,
    DateTime? UpdatedAt,
    DateTime? PublishedAt,
    bool IsDraft,
    string [] Tags);
