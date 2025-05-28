using InkWell.Cli.Tools;

namespace InkWell.Cli.Core;

public interface ITemplateLoader
{
    Task<Dictionary<string, string>> LoadTemplates(string sourcePath);
}

public class TemplateLoader(
    IDirectoryTool directory,
    IFileTool file) : ITemplateLoader
{
    public async Task<Dictionary<string, string>> LoadTemplates(string sourcePath)
    {
        var htmlDir = sourcePath.Path("html");

        Dictionary<string, string> templates = [];
        templates["root"] = await file.ReadAllTextAsync(htmlDir, "index.html");

        var templateDir = htmlDir.Path("templates");

        var htmlTemplates = directory.GetFiles(templateDir, "*.html");

        foreach (var template in htmlTemplates)
        {
            var family = Path.GetFileNameWithoutExtension(template).ToLowerInvariant();
            templates[family] = await file.ReadAllTextAsync(template);
        }

        return templates;
    }
}
