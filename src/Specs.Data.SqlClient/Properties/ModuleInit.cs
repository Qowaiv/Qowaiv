namespace Specs.Properties;

internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Register() => TUnit.ConventionBasedConverters.Register(typeof(ModuleInitializer).Assembly);
}
