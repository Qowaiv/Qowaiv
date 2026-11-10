using Qowaiv.DomainModel;
using Qowaiv.DomainModel.TestTools;
using System.Diagnostics.Contracts;

namespace AwesomeAssertions;

/// <summary>Contains extension methods for custom assertions in unit tests.</summary>
[DebuggerNonUserCode]
public static class QowaivDomainModelAwesomeAssertions
{
    /// <summary>
    /// Returns an <see cref="EventBufferAssertions{TId}"/> object that can be used to assert the
    /// current event buffer.
    /// </summary>
    /// <typeparam name="TId">
    /// The type of the identifier of the event buffer.
    /// </typeparam>
    [Pure]
    [CLSCompliant(false)]
    public static EventBufferAssertions<TId> Should<TId>(this EventBuffer<TId> buffer) => new(buffer);
}
