using Qowaiv.CodeGeneration.Syntax;
using System.IO;

namespace Qowaiv.CodeGeneration;

/// <summary>Writer to generate C# code.</summary>
public sealed class CSharpWriter
{
    /// <summary>UTF-8 BOM.</summary>
    public static readonly Encoding Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    /// <summary>The C# writer settings.</summary>
    public CSharpWriterSettings Settings { get; }

    private readonly TextWriter Writer;
    private int Indentation;

    /// <summary>Initializes a new instance of the <see cref="CSharpWriter"/> class.</summary>
    public CSharpWriter(TextWriter writer) : this(writer, null) { }

    /// <summary>Initializes a new instance of the <see cref="CSharpWriter"/> class.</summary>
    public CSharpWriter(TextWriter writer, CSharpWriterSettings? settings)
    {
        Writer = Guard.NotNull(writer);
        Settings = settings ?? new();
    }

    /// <summary>Writes a character to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(char ch)
    {
        Writer.Write(ch);
        return this;
    }

    /// <summary>Writes code (represented as a string) to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(string? code)
    {
        Writer.Write(code);
        return this;
    }

    /// <summary>Writes the code to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(Code? code)
    {
        code?.WriteTo(this);
        return this;
    }

    /// <summary>Writes the visibility to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(CodeVisibility visibility)
    {
        Writer.Write(visibility.ToString().ToLowerInvariant());
        return this;
    }

    /// <summary>Writes multiple instructions separated by a split action.</summary>
    [FluentSyntax]
    public CSharpWriter Write(IEnumerable<Action<CSharpWriter>> writes, Action<CSharpWriter> split)
    {
        Guard.NotNull(writes);
        Guard.NotNull(split);

        var first = true;

        foreach (var write in writes)
        {
            if (!first) split(this);
            write(this);
            first = false;
        }

        return this;
    }

    /// <summary>Writes a type declaration to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(Type type) => Write(type, attribute: false);

    /// <summary>Writes a type declaration to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(Type type, bool attribute)
    {
        Guard.NotNull(type);
        var withoutNamespace = Settings.GlobalUsings.Contains((Nullable.GetUnderlyingType(type) ?? type).Namespace!);
        var name = type.ToCSharpString(!withoutNamespace);
        return attribute && name.EndsWith("Attribute")
            ? Write(name[..^9])
            : Write(name);
    }

    /// <summary>Writes a literal to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Literal(object? value) => Write(new Literal(value));

    /// <summary>Indents the current line of the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Indent()
    {
        foreach (var code in Enumerable.Repeat(Settings.Indentation, Indentation))
        {
            Write(code);
        }
        return this;
    }

    /// <summary>Writes the line including an ending to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Line(string? line) => Write(line).Line();

    /// <summary>Writes the character including an ending to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Line(char ch) => Write(ch).Line();

    /// <summary>Writes the character including an ending to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Line() => Write(Settings.NewLine);

    /// <summary>Writes a namespace declaration.</summary>
    [Pure]
    public IDisposable NamespaceDeclaration(Namespace @namespace)
    {
        Guard.NotDefault(@namespace);

        if (Settings.UseFileScopedNamespaces)
        {
            Line($"namespace {@namespace};");
            Line();
            return new FileScopedNamespace();
        }
        else
        {
            Line($"namespace {@namespace}");
            return CodeBlock();
        }
    }

    /// <summary>Writes a code block (`{ ... }`).</summary>
    [Pure]
    public IDisposable CodeBlock(string markers = "{}")
    {
        Guard.NotNullOrEmpty(markers);
        Line(markers[0]);
        Indentation++;
        return new ScopedCodeBlock(this, markers[1]);
    }

    /// <summary>
    /// Clears all buffers for the current writer and causes any buffered data
    /// to be written to the code file.
    /// </summary>
    public void Flush() => Writer.Flush();

    private sealed record ScopedCodeBlock(CSharpWriter Writer, char Close) : IDisposable
    {
        /// <inheritdoc />
        public void Dispose()
        {
            Writer.Indentation--;
            Writer.Indent().Line(Close);
        }
    }

    private sealed class FileScopedNamespace : IDisposable
    {
        /// <inheritdoc />
        public void Dispose() { }
    }
}
