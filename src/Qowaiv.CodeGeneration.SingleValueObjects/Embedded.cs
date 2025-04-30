using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.SingleValueObjects;

internal static class Embedded
{
    [Pure]
    public static CodeSnippet Snippet(string name)
    {
        var path = $"Qowaiv.CodeGeneration.SingleValueObjects.Snippets.{name}.cs";

        using var stream = typeof(Embedded).Assembly.GetManifestResourceStream(path);

        return stream is { }
            ? CodeSnippet.Load(stream)
            : throw new ArgumentException($"The path '{path}' is not a stream.", nameof(name));
    }
}
