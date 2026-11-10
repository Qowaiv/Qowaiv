using static ConquerClub.UnitTests.Arrange;
using Commands = ConquerClub.Domain.Commands;

namespace Deploy_specs;

public class Must_be
{
    [Test]
    public void in_deploy_phase()
    {
        var command = new Deploy(
            Netherlands,
            Player.P1.Army(3),
            Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action must be in the Deploy phase to be executed, not in the Attack phase."));
    }

    [Test]
    public void by_active_player()
    {
        var command = new Deploy(
            Netherlands,
            Player.P2.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action can only be applied by the active P1, not by P2."));
    }

    [Test]
    public void to_existing_country()
    {
        var command = new Deploy(
            Unknown,
            Player.P1.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country with id 666 does not exist."));
    }

    [Test]
    public void to_country_owned_by_player()
    {
        var command = new Deploy(
            Belgium,
            Player.P1.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country Belgium must be owned by P1."));
    }

    [Test]
    public void with_army_that_not_exceeds_available()
    {
        var command = new Deploy(
            Netherlands,
            Player.P1.Army(4),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action not be executed as it requires more armies (P1.Army(4)) then available (P1.Army(3))."));
    }
}

public class Adds_armies_to_target
{ 
    [Test]
    public void and_stays_in_deploy_phase_when_deployment_left()
    {
        var command = new Deploy(
            Netherlands,
            Player.P1.Army(2),
            Game_Id,
            ExpectedVersion: 4);

        var game = Handle(command, Benelux()).Should().BeValid().Value;

        game.Countries.ById(Netherlands).Army.Should().Be(Player.P1.Army(5));
        game.ArmyBuffer.Should().Be(Player.P1.Army(1));
        game.Phase.Should().Be(GamePhase.Deploy);
    }

    [Test]
    public void and_moves_to_attack_phase_when_out_deployments()
    {
        var command = new Deploy(
            Netherlands,
            Player.P1.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        var game = Handle(command, Benelux()).Should().BeValid().Value;

        game.Countries.ById(Netherlands).Army.Should().Be(Player.P1.Army(6));
        game.ArmyBuffer.Should().Be(Army.None);
        game.Phase.Should().Be(GamePhase.Attack);
    }
}
