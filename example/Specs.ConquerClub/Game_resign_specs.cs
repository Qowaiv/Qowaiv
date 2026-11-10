using static ConquerClub.UnitTests.Arrange;

namespace Game_specs;

public class Resign_result_in
{
    [Test]
    public void active_player_being_replaced_by_neutral()
    {
        var command = new Resign(Game_Id, 4);
        var buffer = BeneluxWithoutArmies()
            .Add(new ArmiesInitialized
            {
                Armies =
                [
                    Player.P1.Army(1),
                    Player.P2.Army(80),
                    Player.P3.Army(85),
                ]
            })
            .Add(new TurnStarted(Player.P1.Army(3)));
        
        var game = Handle(command, buffer).Should().BeValid().Value;

        game.Phase.Should().Be(GamePhase.Deploy);
        game.Countries.ById(Netherlands).Army.Should().Be(Player.Neutral.Army(1));
        game.ActivePlayer.Should().Be(Player.P2);
        game.ActivePlayers.Should().BeEquivalentTo([Player.P2, Player.P3]);
    }

    [Test]
    public void the_game_being_ended_if_one_player_survives()
    {
        var command = new Resign(Game_Id, 4);

        Handle(command, Benelux()).Should().BeValid()
            .Value.Phase.Should().Be(GamePhase.Finished);
    }
}
