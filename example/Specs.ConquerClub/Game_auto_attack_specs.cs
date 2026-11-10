using static ConquerClub.UnitTests.Arrange;

namespace Game_specs;

public class Auto_attack_can_not_be_applied_when_the
{
    [Test]
    public void current_phase_is_not_attack()
    {
        var command = new AutoAttack(
            Attacker: Netherlands,
            Defender: Belgium,
            Game: Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action must be in the Attack phase to be executed, not in the Deploy phase."));
    }

    [Test]
    public void attacking_country_is_unknown()
    {
        var command = new AutoAttack(
            Attacker: Unknown,
            Defender: Belgium,
            Game: Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy())
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country with id 666 does not exist."));
    }

    [Test]
    public void defending_country_is_unknown()
    {
        var command = new AutoAttack(
            Attacker: Netherlands,
            Defender: Unknown,
            Game: Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country with id 666 does not exist."));
    }

    [Test]
    public void attacking_country_is_not_owned_by_the_attacker()
    {
        var command = new AutoAttack(
            Attacker: Belgium,
            Defender: Luxembourg,
            Game: Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country Belgium must be owned by P1."));
    }

    [Test]
    public void defending_country_is_owned_by_the_attacker()
    {
        var command = new AutoAttack(
            Attacker: Netherlands,
            Defender: Netherlands,
            Game: Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country Netherlands must not be owned by P1."));
    }

    [Test]
    public void defending_country_can_not_be_reached_by_the_attacking_country()
    {
        var command = new AutoAttack(
            Attacker: Netherlands,
            Defender: Luxembourg,
            Game: Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Luxembourg can not be reached from Netherlands."));
    }

    [Test]
    public void attacking_country_has_an_army_size_of_less_then_two()
    {
        var command = new AutoAttack(
            Attacker: Luxembourg,
            Defender: Belgium,
            Game: Game_Id,
            ExpectedVersion: 5);

         Handle(command, BeneluxWithoutArmies()
            .Add(new ArmiesInitialized
            {
                Armies =
            [
                Player.P1.Army(1),
                Player.P2.Army(1),
                Player.P1.Army(1),
            ]
            })
            .Add(new TurnStarted(Player.P1.Army(3)))
            .Deploy())
            
            .Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("County Luxembourg lacks an army to attack."));
    }
}
public class Auto_attack_can_only_be_applied_when
{
    [Test]
    public void none_of_the_guards_fail()
    {
        var command = new AutoAttack(
            Attacker: Netherlands,
            Defender: Belgium,
            Game: Game_Id,
            ExpectedVersion: 5);

        Handle(command, Benelux().Deploy()).Should().BeValid();
    }
}
