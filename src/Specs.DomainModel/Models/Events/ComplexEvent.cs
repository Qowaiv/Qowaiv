namespace Models.Events;

public sealed record class ComplexEvent(int Value, string Message, DateTime Date);
