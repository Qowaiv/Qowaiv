namespace ConquerClub.Domain.Events;

public sealed record SettingsInitialized(int Players, int RoundLimit, bool FogOfWar);
