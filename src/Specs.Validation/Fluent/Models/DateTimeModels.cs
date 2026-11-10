namespace Specs.Fluent.Models;

public class DateTimeModel
{
    public DateTime Prop { get; set; }
}

public class DateModel
{
    public Date Prop { get; set; }
}

public class DateOnlyModel
{
    public DateOnly Prop { get; set; }
}

public class NullableDateModel
{
    public Date? Prop { get; set; }
}
public class NullableDateOnlyModel
{
    public DateOnly? Prop { get; set; }
}

public class NullableDateTimeModel
{
    public DateTime? Prop { get; set; }
}
