namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code literal.</summary>
public sealed class Literal : Code
{
    /// <summary>Initializes a new instance of the <see cref="Literal"/> class.</summary>
    public Literal(object? value) => Value = value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly object? Value;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);
        _ = Value switch
        {
            Nill or
            null /*.........*/ => writer.Write("null"),
            Type type /*....*/ => writer.Write("typeof(").Write(type).Write(')'),
            bool boolean /*.*/ => writer.Write(boolean ? "true" : "false"),
            int int32 /*....*/ => writer.Write(Int32(int32)),
            double dbl /*...*/ => writer.Write(Double(dbl)),
            decimal dec /*..*/ => writer.Write(dec.ToString(CultureInfo.InvariantCulture)).Write('m'),
            string str /*...*/ => writer.Write(String(str)),
            Enum @enum /*...*/ => writer.Write(@enum.GetType()).Write('.').Write(@enum.ToString()),
            _ => throw new NotSupportedException($"Literals of type {Value.GetType()} are not supported"),
        };
    }

    [Pure]
    private static string Int32(int num) => num switch
    {
        int.MinValue /*...*/ => "int.MinValue",
        int.MaxValue /*...*/ => "int.MaxValue",
        _ => num.ToString(CultureInfo.InvariantCulture),
    };

    [Pure]
    private static string Double(double dbl) => dbl switch
    {
        double.MinValue /*...*/ => "double.MinValue",
        double.MaxValue /*...*/ => "double.MaxValue",
        _ when double.IsNaN(dbl) /*...*/ => "double.NaN",
        _ when double.IsPositiveInfinity(dbl) /*...*/ => "double.PositiveInfinity",
        _ when double.IsNegativeInfinity(dbl) /*...*/ => "double.NegativeInfinity",
        _ => dbl.ToString(CultureInfo.InvariantCulture),
    };

    [Pure]
    private static string String(string str)
    {
        return str.Contains('\\') || str.Contains('"')
            ? $@"@""{str}"""
            : $@"""{str}""";
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
