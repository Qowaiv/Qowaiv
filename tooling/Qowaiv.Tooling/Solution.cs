namespace Qowaiv.Tooling;

public static class Solution
{
    public static readonly DirectoryInfo Root = new("../../../../../");

    [Pure]
    public static FileInfo File(this DirectoryInfo directory, string file) => new(Path.Combine(directory.FullName, file));
}
