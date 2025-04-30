using Microsoft.CodeAnalysis;
using System.Threading;

namespace Qowaiv.CodeGeneration.SingleValueObjects;

[Generator]
internal sealed class SvoGenerator : BaseGenerator<SvoParameters>
{
    protected override string MetadataName => "Qowaiv.Customization.SvoAttribute`1";

    /// <summary>Collects the SVO parameters.</summary>
    [Pure]
    protected override SvoParameters Collect(GeneratorAttributeSyntaxContext context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var symbol = (INamedTypeSymbol)context.TargetSymbol;
        var attr = context.Attributes.First(m => $"{m.AttributeClass?.ContainingNamespace}.{m.AttributeClass?.MetadataName}" == MetadataName).AttributeClass!;

        return new()
        {
            Svo = symbol.Name,
            Behavior = FullName(attr.TypeArguments[0]),
            Namespace = symbol.ContainingNamespace.ToString(),
        };
    }

    /// <summary>Generates the source code for the SVO.</summary>
    [Pure]
    protected override Code Generate(SourceProductionContext context, SvoParameters parameters)
        => new SvoTemplate(parameters);
}
