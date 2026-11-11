using System.Text;

namespace Qowaiv.DomainModel.TestTools;

/// <summary>Assertions on <see cref="EventBuffer{TId}"/>.</summary>
/// <typeparam name="TId">
/// The type of the identifier of the event buffer.
/// </typeparam>
[Inheritable]
[CLSCompliant(false)]
public class EventBufferAssertions<TId>(EventBuffer<TId> subject)
{
    /// <summary>The subject under test.</summary>
    public EventBuffer<TId> Subject { get; } = subject;

    /// <summary>Verifies that the <see cref="Aggregate{TAggregate}"/> has the expected uncommitted events.</summary>
    /// <param name="uncommitted">
    /// The expected uncommitted event messages.
    /// </param>
    [Assertion]
    [DebuggerStepThrough]
    public void HaveUncommittedEvents(params object[] uncommitted)
    {
        Guard.HasAny(uncommitted);

        var sb = new StringBuilder();
        var failure = false;
        var offset = Subject.CommittedVersion;
        var actualEvents = Subject.Uncommitted.ToArray();
        var shared = Math.Min(uncommitted.Length, actualEvents.Length);

        if (actualEvents.Length == 0)
        {
            throw new AssertionFailed("There where no uncommitted events.");
        }

        for (var i = 0; i < shared; i++)
        {
            failure |= sb.AppendEvents(offset + i, uncommitted[i], actualEvents[i]);
        }

        failure |= sb.AppendExtraEvents(actualEvents, offset, shared, "Extra:  ");
        failure |= sb.AppendExtraEvents(uncommitted, offset, shared, "Missing: ");

        if (failure)
            throw new AssertionFailed(sb.Insert(0, "The uncommitted events where different than expected." + Environment.NewLine).ToString());
    }
}
