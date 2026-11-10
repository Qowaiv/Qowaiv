using Qowaiv.DomainModel;

namespace Supported_EventTypes_specs;

public class Exposes
{
    [Test]
    public void Supported_EventTypes_of_protected_dispatcher()
        => Aggregate.SupportedEventTypes<SimpleEventSourcedAggregate>()
        .Should().BeEquivalentTo(
        [
            typeof(NameUpdated),
            typeof(DateOfBirthUpdated),
            typeof(SimpleInitEvent),
            typeof(InvalidEvent),
        ]);
}
