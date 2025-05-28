using System;
using System.IO;

namespace InkWell.Cli.Tools;

public interface IDirectoryTool
{
    string[] GetDirectories(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly);

    string[] GetFiles(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly);
}

public class DirectoryTool : IDirectoryTool
{
    public string[] GetDirectories(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return Directory.GetDirectories(path, searchPattern, searchOption);
    }

    public string[] GetFiles(
        string path,
        string searchPattern = "*",
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        return Directory.GetFiles(path, searchPattern, searchOption);
    }
}

