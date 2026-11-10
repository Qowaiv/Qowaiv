namespace Specs.Properties;

internal static class ModuleInit
{
    [ModuleInitializer]
    public static void Register() => TUnit.ConventionBasedConverters.Register(typeof(ModuleInit).Assembly);
}
