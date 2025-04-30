using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.SingleValueObjects;

internal sealed class SvoTemplate(SvoParameters parameters) : Code
{
    private static readonly CodeSnippet Snippet = Embedded.Snippet("CustomSvo");

    private readonly SvoParameters Parameters = parameters;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer) => Guard.NotNull(writer)
        .Write(Snippet
            .Transform(line => line
                .Replace("@Svo", Parameters.Svo)
                .Replace("@Behavior", Parameters.Behavior)
                .Replace("@Namespace", Parameters.Namespace.ToString())));

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
