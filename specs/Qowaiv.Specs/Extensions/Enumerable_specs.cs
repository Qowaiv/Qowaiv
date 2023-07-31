namespace Extensions.Enumerable_specs;

public class Average
{
    [Test]
    public void Elo()
    {
        var elos = new Elo[] { 1400, 1600 };
        elos.Average().Should().Be(1500.Elo());
    }
}
