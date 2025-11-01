using Microsoft.CodeAnalysis;
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
        var symbols = context.SyntaxProvider.ForAttributeWithMetadataName(MetadataName, Filter, Collect);
        context.RegisterSourceOutput(symbols, GenerateInternal);
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
    private void GenerateInternal(SourceProductionContext context, TParameters pars)
    {
        var code = Generate(context, pars);
        var ns = pars.Namespace.IsEmpty() ? "__global__" : pars.Namespace.ToString();
        context.AddSource($"{ns}.{pars.Svo}.g.cs", code.ToString());
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
