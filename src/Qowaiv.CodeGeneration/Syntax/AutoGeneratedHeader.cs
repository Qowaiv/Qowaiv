using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>
/// Provides the header for generated files.
/// </summary>
public sealed class AutoGeneratedHeader : Code
{
    /// <summary>Initializes a new instance of the <see cref="AutoGeneratedHeader"/> class.</summary>
    internal AutoGeneratedHeader(IEnumerable<Assembly>? toolchain)
    {
        var all = toolchain ?? GetToolchain();
        Toolchain = [.. all.Distinct()];
    }

    /// <summary>
    /// Gets the assemblies used for the code generation.
    /// </summary>
    public IReadOnlyList<Assembly> Toolchain { get; }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer)
            .Line("// ------------------------------------------------------------------------------")
            .Line("// <auto-generated>")
            .Line("//     This code was generated by a tool.");

        var longestName = Toolchain.Any()
            ? Toolchain.Select(asm => asm.GetName().Name).Max(n => n?.Length ?? 0)
            : 0;

        foreach (var assembly in Toolchain)
        {
            var name = assembly.GetName().Name;

            if (string.IsNullOrWhiteSpace(name))
            {
                continue;
            }

            var paddingSize = longestName - name.Length;
            var padding = new string(' ', paddingSize);

            var version = GetVersion(assembly);
            writer.Line($"//     - {name}{padding} {version}");
        }

        writer
            .Line("//")
            .Line("//     Changes to this file may cause incorrect behavior and will be lost if")
            .Line("//     the code is regenerated.")
            .Line("// </auto-generated>")
            .Line("// ------------------------------------------------------------------------------")
            .Line();
    }

    [Pure]
    private static string GetVersion(Assembly assembly)
    {
        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        if (attribute is { })
        {
            return attribute.InformationalVersion.Split('+')[0];
        }

        return assembly.GetName().Version?.ToString() ?? "0.0.0.0";
    }

    private static readonly string[] IgnoredPrefixes =
    [
        "System.",
        "nunit.",
        "Xunit",
        "Microsoft.",
    ];

    [Pure]
    private static Assembly[] GetToolchain()
    {
        var result = new List<Assembly>();
        var trace = new StackTrace();
        var frames = trace.GetFrames();

        foreach (var frame in frames)
        {
            var asm = frame.GetMethod()?.DeclaringType?.Assembly;

            if (asm is { })
            {
                result.Add(asm);
            }
        }

        return [.. result.Distinct().Where(HasAllowedPrefix)];
    }

    [Pure]
    private static bool HasAllowedPrefix(Assembly asm)
        => HasAllowedPrefix(asm.GetName().Name);

    [Pure]
    private static bool HasAllowedPrefix(string? asm)
        => asm is { Length: > 0 }
        && !IgnoredPrefixes.Any(prefix => asm.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
}
