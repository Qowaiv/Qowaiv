namespace ConquerClub.Domain;

public enum GamePhase
{
    None = 0,

    /// <summary>The deploy phase.</summary>
    Deploy,

    /// <summary>The attack phase.</summary>
    Attack,

    /// <summary>The advance phase.</summary>
    Advance,

    /// <summary>The game has been finished.</summary>
    Reinforce,

    /// <summary>The reinforce phase.</summary>
    Finished,
}
