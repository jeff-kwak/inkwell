namespace InkWell.Cli.Commands.Compile;

public static class TemplateDataExtensions
{
    public static string? Iso8601(this DateTime? dateTime)
    {
        return dateTime?.ToString("yyyy-MM-ddTHH:mm:ss");
    }
}
