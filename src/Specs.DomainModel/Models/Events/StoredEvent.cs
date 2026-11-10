namespace Models.Events;

public sealed record StoredEvent(object Id, long Version, object Payload);
