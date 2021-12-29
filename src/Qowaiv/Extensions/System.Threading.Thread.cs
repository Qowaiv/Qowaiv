using Qowaiv.Threading;

namespace System.Threading;

/// <summary>Extensions on <see cref="Thread"/>.</summary>
public static class QowaivThreadExtensions
{
    /// <summary>Gets the value of T.</summary>
    [Pure]
    public static T? GetValue<T>(this Thread thread) => ThreadDomain.Current.Get<T>();

    /// <summary>Sets the value for T.</summary>
    public static void SetValue<T>(this Thread thread, T value) => ThreadDomain.Current.Set(value);

    /// <summary>Removes the value of T.</summary>
    public static void RemoveValue(this Thread thread, Type type) => ThreadDomain.Current.Remove(type);
}
