namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a parse error.</summary>
public sealed class ParseError : InvalidOperationException
{
    /// <summary>Initializes a new instance of the <see cref="ParseError"/> class.</summary>
    public ParseError() { }

    /// <summary>Initializes a new instance of the <see cref="ParseError"/> class.</summary>
    public ParseError(string? message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="ParseError"/> class.</summary>
    public ParseError(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>Creates a line based parse error.</summary>
    [Pure]
    public static ParseError Line(int lineNr, string line, string message) => new($"[{lineNr}] {line}: {message}");
}
