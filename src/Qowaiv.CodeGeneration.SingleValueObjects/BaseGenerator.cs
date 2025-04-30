using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Threading;

namespace Qowaiv.CodeGeneration.SingleValueObjects;

/// <summary>Provides a base generator for generating SVO's.</summary>
/// <typeparam name="TParameters">
/// The type of the parameters.
/// </typeparam>
public abstract class BaseGenerator<TParameters> : IIncrementalGenerator
    where TParameters : BaseParameters
{
    /// <summary>The metadata name of the attribute to trigger on.</summary>
    protected abstract string MetadataName { get; }

    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var symbols = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataName, Filter, Collect).Collect();
        context.RegisterSourceOutput(symbols, Generate);
    }

    /// <summary>Collects the SVO parameters.</summary>
    [Pure]
    protected abstract TParameters Collect(GeneratorAttributeSyntaxContext context, CancellationToken token);

    /// <summary>Filters syntax nodes that should be included (default all are included).</summary>
    [Pure]
    protected virtual bool Filter(SyntaxNode node, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return true;
    }

    /// <summary>Generates the source code for the SVO.</summary>
    protected void Generate(SourceProductionContext context, ImmutableArray<TParameters> parameters)
    {
        foreach (var pars in parameters)
        {
            var code = Generate(context, pars);
            context.AddSource($"{pars.Namespace}.{pars.Svo}.g.cs", code.ToString());
        }
    }

    /// <summary>Generates <see cref="Code"/> based on the parameters.</summary>
    [Pure]
    protected abstract Code Generate(SourceProductionContext context, TParameters parameters);

    /// <summary>Gets the full name of <see cref="ITypeSymbol"/>.</summary>
    [Pure]
    protected static string FullName(ITypeSymbol symbol)
        => symbol.ContainingType is ITypeSymbol containing
        ? $"{FullName(containing)}.{symbol.Name}"
        : $"{symbol.ContainingNamespace}.{symbol.Name}";
}
