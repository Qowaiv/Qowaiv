using Microsoft.CodeAnalysis;
using System.Threading;

namespace Qowaiv.CodeGeneration.SingleValueObjects;

[Generator]
public sealed class IdGenerator : BaseGenerator<IdParameters>
{
    /// <inheritdoc />
    protected override string MetadataName => "Qowaiv.Customization.IdAttribute`2";

    /// <inheritdoc />
    [Pure]
    protected override IdParameters Collect(GeneratorAttributeSyntaxContext context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var symbol = (INamedTypeSymbol)context.TargetSymbol;
        var attr = context.Attributes.First(m => $"{m.AttributeClass?.ContainingNamespace}.{m.AttributeClass?.MetadataName}" == MetadataName).AttributeClass!;

        return new()
        {
            Svo = symbol.Name,
            Behavior = FullName(attr.TypeArguments[0]),
            Value = FullName(attr.TypeArguments[1]),
            Namespace = symbol.ContainingNamespace.ToString(),
        };
    }

    /// <inheritdoc />
    [Pure]
    protected override Code Generate(SourceProductionContext context, IdParameters parameters)
        => new IdTemplate(parameters);
}
