# TUnit convetion based conversions
When running test with [`TUnit`](https://tunit.dev), test arguments provided with
`[Arguments(..)]` are not automatically converted using a `TypeConverter`. By
registering some custom converters, provided by `Qowaiv.TestTools` this omissions
can be resolved:

``` C#
internal static class ConventionBasedConverters
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void Register()
    {
        foreach (var (source, target) in TUnitTypeConversions.Scan(/* assembly to scan */))
            AotConverterRegistry.Register(new Converter(new(source, target)));
    }

    private sealed class Converter(ConventionBasedConverter converter) : IAotConverter
    {
        public Type SourceType => converter.SourceType;
        public Type TargetType => converter.TargetType;
        public object? Convert(object? value) => converter.Convert(value);
    }
}

```

## ConventionBasedConverter
The convetion based converter converts with the following priority:
1. `System.ComponentModel.TypeConverter`
2. `implicit` or `explict` cast
3. `System.Convert.ChangeType(object, Type, IFormatProvider)`
