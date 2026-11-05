using Qowaiv.Formatting;
using System.IO;

namespace Qowaiv.TestTools;

/// <summary>Helper to reference null values.</summary>
public static class Nil
{
    /// <summary>Gets a null <see cref="System.IO.FileInfo" />.</summary>
    public static readonly FileInfo? FileInfo = null;

    /// <summary>Gets a null <see cref="Formatting.FormattingArgumentsCollection" />.</summary>
    public static readonly FormattingArgumentsCollection? FormattingArgumentsCollection = null;

    /// <summary>Gets a null <see cref="System.IFormattable" />.</summary>
    public static readonly IFormattable? IFormattable = null;

    /// <summary>Gets a null <see cref="string" />.</summary>
    public static readonly string? String = null;

    /// <summary>Gets a null <see cref="object" />.</summary>
    public static readonly object? Object = null;
}
