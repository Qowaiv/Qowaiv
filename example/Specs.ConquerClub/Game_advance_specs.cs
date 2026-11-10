using static ConquerClub.UnitTests.Arrange;

namespace Game_specs;

public class Advance_can_not_be_applied_when_the
{
    [Test]
    public void current_phase_is_not_advance()
    {
        var command = new Advance(
              To: Player.P1.Army(3),
              Game: Game_Id,
              ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action must be in the Advance phase to be executed, not in the Deploy phase."));
    }

    [Test]
    public void army_of_not_active_player()
    {
        var command = new Advance(
              To: Player.P2.Army(3),
              Game: Game_Id,
              ExpectedVersion: 5);

        Handle(command, Benelux().Add(new Conquered(Netherlands, Belgium)))
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action can only be applied by the active P1, not by P2."));
    }

    [Test]
    [MatrixDataSource]
    public void advancement_exceeds_available([MatrixRange<int>(2, 4)] int size)
    {
        var command = new Advance(
              To: Player.P1.Army(size),
              Game: Game_Id,
              ExpectedVersion: 5);

        Handle(command, Benelux().Add(new Conquered(Netherlands, Belgium)))
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error($"Action not be executed as it requires more armies (P1.Army({size})) then available (P1.Army(1))."));
    }
}

public class Advance_can_only_be_applied_when
{
    [Test]
    public void advancement_exceeds_available()
    {
        var command = new Advance(
              To: Player.P1.Army(1),
              Game: Game_Id,
              ExpectedVersion: 5);

        Handle(command, Benelux().Add(new Conquered(Netherlands, Belgium)))
            .Should().BeValid()
            .And.Value.ArmyBuffer.Should().Be(Army.None);
    }
}
