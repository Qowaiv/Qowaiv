using Qowaiv.OpenApi;
using Qowaiv.Reflection;
using System.Runtime.CompilerServices;

namespace Qowaiv.TestTools.Assertions;

internal static class Assertion
{
    extension(StringBuilder sb)
    {
        [DoesNotReturn]
        public void Fail() => throw new AssertionFailed(sb.ToString());

        [FluentSyntax]
        public StringBuilder Value(object? value) => value switch
        {
            null => sb.Null(),
            Type v => sb.Value(v),
            string v => sb.Value(v),
            IReadOnlyCollection<string?> v => sb.Value(v),
            OpenApiDataType v => sb.Value(v),
            _ => sb.Append(value),
        };

        [FluentSyntax]
        public StringBuilder Value(Type? value)
            => value is null
            ? sb.Null()
            : sb.Append(value.ToCSharpString());

        [FluentSyntax]
        public StringBuilder Value(string? value)
            => value is null
            ? sb.Null()
            : sb.Append('"').Append(value.Replace("\"", "\\\"")).Append('"');

        [FluentSyntax]
        public StringBuilder Value(IReadOnlyCollection<string?>? values)
        {
            if (values is null) return sb.Null();
            sb.Append('[');

            var next = false;

            foreach (var value in values)
            {
                if (next) sb.Append(", ");
                else next = true;

                sb.Value(value);
            }
            return sb.Append(']');
        }

        [FluentSyntax]
        public StringBuilder Value(OpenApiDataType? value) => value is null
            ? sb.Null()
            : sb
            .Line('{')
            .Append("    DataType = ").Value(value.DataType).Line(',')
            .Append("    Description = ").Value(value.Description).Line(',')
            .Append("    Type = ").Value(value.Type).Line(',')
            .Append("    Example = ").Value(value.Example).Line(',')
            .Append("    Format = ").Value(value.Format).Line(',')
            .Append("    Pattern = ").Value(value.Pattern).Line(',')
            .Append("    Enum = ").Value(value.Enum).Line(',')
            .Append('}');

        [FluentSyntax]
        public StringBuilder Null() => sb.Append("<null>");

        [FluentSyntax]
        public StringBuilder Line() => sb.AppendLine();

        [FluentSyntax]
        public StringBuilder Line(string value) => sb.Append(value).AppendLine();

        [FluentSyntax]
        public StringBuilder Line(char value) => sb.Append(value).AppendLine();
    }

    [Pure]
    public static StringBuilder New() => new();
}
