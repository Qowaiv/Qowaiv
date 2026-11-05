#if NET48
namespace System
{

    internal static class QowaivTypeExtensions
    {
        public static bool IsAssignableTo(this Type type, Type other)
            => other.IsAssignableFrom(type);
    }
}
#endif
