namespace Qowaiv.TestTools;

public class SingleValueObjectSpecs
{
    protected SingleValueObjectSpecs() { }

    public static IEnumerable<Type> AllSvos
        => AppDomain.CurrentDomain.GetAssemblies()
        .Where(assembly => assembly.FullName!.Contains("Qowaiv"))
        .SelectMany(assembly => assembly.GetExportedTypes())
        .Where(type
            => type.GetCustomAttributes<SingleValueObjectAttribute>().Any()
            && !type.GetCustomAttributes<ObsoleteAttribute>().Any());

    public static IEnumerable<Type> AllSvosExceptGeneric
        => AllSvos.Where(svo => !svo.IsGenericType);

    public static IEnumerable<Type> SvosWithEmpty
        => AllSvos.Where(svo
            => !svo.IsGenericType
            && svo.GetCustomAttribute<SingleValueObjectAttribute>() is { } attr
            && attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasEmptyValue));

    public static IEnumerable<Type> SvosWithUnknown
        => AllSvos.Where(svo
            => !svo.IsGenericType
            && svo.GetCustomAttribute<SingleValueObjectAttribute>() is { } attr
            && attr.StaticOptions.HasFlag(SingleValueStaticOptions.HasUnknownValue));

    public static IEnumerable<Type> JsonSerializable
        => AllSvos
        .Where(IsJsonSerializable)
        .Except(
        [
            typeof(Secret),
            typeof(CryptographicSeed)
        ]);

    private static bool IsJsonSerializable(Type type)
        => type
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Exists(m => m.Name == nameof(Date.FromJson)
                && m.ReturnType == type
                && m.GetParameters().Length == 1
                && m.GetParameters()[0].ParameterType == typeof(string));
}
