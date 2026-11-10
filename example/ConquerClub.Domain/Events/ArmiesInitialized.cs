namespace ConquerClub.Domain.Events;

public sealed record ArmiesInitialized(params Army[] Armies);
