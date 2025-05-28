namespace InkWell.Cli.Tools;

public static class SequenceExtensions
{
    public static void Each<T>(this IEnumerable<T> sequence, Action<T> action)
    {
        foreach (var item in sequence)
        {
            action(item);
        }
    }
}
