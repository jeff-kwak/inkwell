using Markdig.Extensions.Yaml;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace InkWell.Cli.Tools;

public interface IYamlTool
{
    Dictionary<string, object> Deserialize(YamlFrontMatterBlock yamlBlock);
}

public class YamlTool : IYamlTool
{
    public Dictionary<string, object> Deserialize(YamlFrontMatterBlock yamlBlock)
    {
        // TODO: move this into the configuration
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        return deserializer.Deserialize<Dictionary<string, object>>(yamlBlock.Lines.ToString());
    }
}
