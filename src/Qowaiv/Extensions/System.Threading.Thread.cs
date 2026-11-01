using Qowaiv.Threading;

namespace Qowaiv.Extensions;

/// <summary>Extensions on <see cref="Thread" />.</summary>
public static class QowaivThread
{
    extension(Thread thread)
    {
        /// <summary>Gets the value of T.</summary>
        [Pure]
        [SuppressMessage("Performance", "CA1822", Justification = "Extension members can not be static.")]
        public T? GetValue<T>() => ThreadDomain.Current.Get<T>();

        /// <summary>Sets the value for T.</summary>
        [SuppressMessage("Performance", "CA1822", Justification = "Extension members can not be static.")]
        [SuppressMessage("Critical Code Smell", "S2325", Justification = "Extension members can not be static.")]
        public void SetValue<T>(T value) => ThreadDomain.Current.Set(value);

        /// <summary>Removes the value of T.</summary>
        [SuppressMessage("Performance", "CA1822", Justification = "Extension members can not be static.")]
        [SuppressMessage("Critical Code Smell", "S2325", Justification = "Extension members can not be static.")]
        public void RemoveValue(Type type) => ThreadDomain.Current.Remove(type);
    }
}
