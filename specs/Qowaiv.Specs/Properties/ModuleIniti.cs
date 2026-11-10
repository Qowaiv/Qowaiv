namespace Specs.Properties;

internal static class ModuleIniti
{
    [ModuleInitializer]
    public static void Register() => TUnit.ConventionBasedConverters.Register(typeof(ModuleIniti).Assembly);
}
