namespace Qowaiv.CodeGeneration;

/// <summary>Settings for writing C# code.</summary>
public sealed record CSharpWriterSettings
{
    /// <summary>Available (global) namespace usings.</summary>
    public IReadOnlyCollection<Namespace> GlobalUsings { get; init; } = [];

    /// <summary>New line character(s).</summary>
    public string NewLine { get; init; } = "\r\n";

    /// <summary>Indentation character(s).</summary>
    public string Indentation { get; init; } = "    ";

    /// <summary>Generates the required modifier for required properties.</summary>
    public bool UseRequiredModifier { get; init; } = true;

    /// <summary>Indicates if namespace declarations should being file scoped.</summary>
    public bool UseFileScopedNamespaces { get; init; } = true;

    /// <summary>File encoding (UTF-8 without BOM).</summary>
    public Encoding Encoding { get; init; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
}
