using static ConquerClub.UnitTests.Arrange;
using Commands = ConquerClub.Domain.Commands;

namespace Game_specs;

public class Start_Game
{
    [Test]
    public void With_two_players_includes_third_neutral_player()
    {
        var command = new Start(
            Game: Game_Id,
            Players: 2,
            RoundLimit: 10,
            Continents:
            [
                new Commands.Continent(
                    Name: "Benelux",
                    Bonus: 3,
                    Territories:
                    [
                        Netherlands,
                        Belgium,
                        Luxembourg,
                    ]),
            ],
            Countries:
            [
                new Commands.Country("Netherlands", [Belgium]),
                new Commands.Country("Belgium", [Netherlands, Luxembourg]),
                new Commands.Country("Luxembourg", [Belgium]),
            ]);

        Handle(command).Should().BeValid()
            .Value.Countries.Select(c => c.Owner).Should().BeEquivalentTo(
        [
            Player.P1,
            Player.P2,
            Player.Neutral
        ]);
    }
}

public class Deploy
{
    [Test]
    public void Can_not_be_applied_by_not_active_player()
    {
        var command = new Commands.Deploy(
            Luxembourg,
            Player.P2.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Action can only be applied by the active P1, not by P2."));
    }

    [Test]
    public void Can_be_applied_by_active_player()
    {
        var command = new Commands.Deploy(
            Netherlands,
            Player.P1.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeValid()
            .Value.Countries.ById(Netherlands).Army.Should().Be(Player.P1.Army(6));
    }

    [Test]
    public void Can_only_be_applied_on_own_country()
    {
        var command = new Commands.Deploy(
            Luxembourg,
            Player.P1.Army(3),
            Game_Id,
            ExpectedVersion: 4);

        Handle(command, Benelux()).Should().BeInvalid()
            .WithMessage(ValidationMessage.Error("Country Luxembourg must be owned by P1."));
    }
}
