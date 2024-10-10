using Microsoft.CodeAnalysis;
using System.IO;
using System.Reflection;

namespace Qowaiv.Diagnostics.Contracts;

[Generator]
internal sealed class Generator : IIncrementalGenerator
{
    private static readonly Assembly Assembly = typeof(Generator).Assembly;

    public void Initialize(IncrementalGeneratorInitializationContext context)
        => context.RegisterPostInitializationOutput(Generate);

    private static void Generate(IncrementalGeneratorPostInitializationContext context)
    {
        foreach (var resource in Assembly.GetManifestResourceNames())
        {
            Generate(context, resource);
        }
    }

    private static void Generate(IncrementalGeneratorPostInitializationContext context, string resourceName)
    {
        using var stream = Assembly.GetManifestResourceStream(resourceName);
        using var streamReader = new StreamReader(stream);
        var text = streamReader.ReadToEnd();

        context.AddSource(resourceName, text);
    }
}
