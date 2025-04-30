using System.IO;
using System.Text.RegularExpressions;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code snippet.</summary>
public sealed class CodeSnippet : Code
{
    /// <summary>Initializes a new instance of the <see cref="CodeSnippet"/> class.</summary>
    internal CodeSnippet(IReadOnlyList<string> lines) => Lines = lines;

    /// <summary>Gets the individual lines of the code snippet.</summary>
    public IReadOnlyList<string> Lines { get; }

    /// <summary>Returns a transformed code snippet.</summary>
    [Pure]
    public CodeSnippet Transform(Func<string, string> transformLine)
    {
        Guard.NotNull(transformLine);
        return new(Lines.Select(transformLine).ToArray());
    }

    /// <summary>Transforms a code snippet.</summary>
    /// <param name="constants">
    /// The available constants.
    /// </param>
    [Pure]
    public CodeSnippet Transform(IReadOnlyCollection<Constant> constants)
    {
        Guard.NotNull(constants);

        var lines = new List<string>();

        var enabled = true;
        var mode = Mode.None;

        var nr = 0;

        foreach (var line in Lines)
        {
            nr++;

            if (Matches(mode, nr, line, "#if", Patterns.If, m => m is not Mode.None, out var @if))
            {
                enabled = Enabled(@if, constants);
                mode = Mode.@if;
            }
            else if (Matches(mode, nr, line, "#elif", Patterns.ElIf, m => m != Mode.@if && m != Mode.elif, out var elif))
            {
                enabled = !enabled && Enabled(elif, constants);
                mode = Mode.elif;
            }
            else if (Matches(mode, nr, line, "#else", Patterns.Else, m => m != Mode.@if && m != Mode.elif, out _))
            {
                enabled = !enabled;
                mode = Mode.@else;
            }
            else if (Matches(mode, nr, line, "#endif", Patterns.EndIf, m => m is Mode.None, out _))
            {
                enabled = true;
                mode = Mode.None;
            }
            else if (enabled && (line != string.Empty || lines.LastOrDefault() != string.Empty))
            {
                lines.Add(line);
            }
        }

        if (mode != Mode.None)
        {
            throw ParseError.Line(nr, Lines[^1], "Missing closing #endif statement.");
        }

        return new([.. lines]);

        static bool Enabled(Match match, IReadOnlyCollection<Constant> constants)
        {
            var enabled = constants.Contains(match.Groups["constant"].Value);
            return match.Groups["negate"].Value is { Length: 1 }
                ? !enabled
                : enabled;
        }

        static bool Matches(Mode mode, int lineNr, string line, string startsWith, Regex pattern, Func<Mode, bool> unexpected, out Match match)
        {
            if (!line.StartsWith(startsWith, StringComparison.Ordinal))
            {
                match = null!;
                return false;
            }
            else if (pattern.Match(line) is { Success: true } m)
            {
                match = m;
                var exec = match.Groups["exec"].Value is { Length: > 0 };

                if (exec && unexpected(mode)) throw ParseError.Line(
                    lineNr,
                    line,
                    mode == Mode.None ? $"Unexpected {startsWith}." : $"Unexpected {startsWith} after #{mode}");

                return exec;
            }
            else throw ParseError.Line(lineNr, line, "invalid pattern");
        }
    }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);
        writer.Write(Lines.Select(Line), writer => writer.Line());
        static Action<CSharpWriter> Line(string line) => writer => writer.Write(line);
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();

    /// <summary>Joins two code snippets.</summary>
    [Pure]
    public static CodeSnippet operator +(CodeSnippet l, CodeSnippet r) => new([..l.Lines, ..r.Lines]);

    /// <summary>Creates a code snippet from a stream.</summary>
    [Pure]
    public static CodeSnippet Load(Stream stream)
    {
        using var reader = new StreamReader(stream);
        var lines = new List<string>();
        while (reader.ReadLine() is { } line)
        {
            lines.Add(line);
        }
        return new(lines.ToArray());
    }

    /// <summary>Parses the code snippet for a string.</summary>
    [Pure]
    public static CodeSnippet Parse(string s)
    {
        Guard.NotNullOrEmpty(s);

        using var reader = new StringReader(s);
        var lines = new List<string>();
        while (reader.ReadLine() is { } line)
        {
            lines.Add(line);
        }
        return new(lines.ToArray());
    }

    private enum Mode
    {
        None = 0,
        @if,
        elif,
        @else,
    }

    private static class Patterns
    {
        public static readonly RegexOptions Options = RegexOptions.Compiled | RegexOptions.CultureInvariant;
        public static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(1);

        public static readonly Regex If = new("^#if +(?<negate>!)?(?<constant>[A-Za-z0-9_]+) *(?<exec>// *exec)? *$", Options, Timeout);
        public static readonly Regex ElIf = new("^#elif +(?<negate>!)?(?<constant>[A-Za-z0-9_]+) *(?<exec>// *exec)? *$", Options, Timeout);
        public static readonly Regex Else = new("^#else *(?<exec>// *exec)? *$", Options, Timeout);
        public static readonly Regex EndIf = new("^#endif *(?<exec>// *exec)? *$", Options, Timeout);
    }
}
