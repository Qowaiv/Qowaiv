using System.IO;

namespace Qowaiv.CodeGeneration;

/// <summary>Extensions on <see cref="Code"/>.</summary>
public static class CodeExtensions
{
    /// <summary>Represents the <see cref="Code"/> as <see cref="string"/>.</summary>
    /// <remarks>
    /// Used by several code syntaxes their <see cref="object.ToString()"/> implementations.
    /// </remarks>
    [Pure]
    public static string Stringify(this Code code)
    {
        Guard.NotNull(code);

        using var text = new StringWriter();
        var writer = new CSharpWriter(text);
        code.WriteTo(writer);
        return text.ToString();
    }
}
