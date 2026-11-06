using Qowaiv.TestTools.Conversion;
using System.Reflection;

namespace Specs.TUnit;

public static class ConventionBasedConverters
{
    /// <summary>
    /// Out-of-the box, TUnit does not convert arguments based on TypeConverters,
    /// with this regestry, it does.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the assembly that should be registered.
    /// </typeparam>
    public static void Register(Assembly assembly)
    {
        foreach (var (source, target) in TUnitTypeConversions.Scan(assembly))
            AotConverterRegistry.Register(new Converter(new(source, target)));
    }

    private sealed class Converter(ConventionBasedConverter converter) : IAotConverter
    {
        public Type SourceType => converter.SourceType;
        public Type TargetType => converter.TargetType;
        public object? Convert(object? value) => converter.Convert(value);
    }
}
