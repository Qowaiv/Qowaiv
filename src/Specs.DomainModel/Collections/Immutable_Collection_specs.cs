namespace Collections.Immutable_Collection_specs;

internal static class Help
{
    public static readonly bool? NotTrue = default;
    public static Dummy FailingCreation() => throw new DivideByZeroException();
}

[EmptyTestClass]
internal class Dummy { }

[EmptyTestClass]
internal class Other { }

internal class Dummies : IEnumerable<Dummy>
{
    public int Count { get; private set; }

    public IEnumerator<Dummy> GetEnumerator()
        => Enumerable.Repeat(new Dummy(), ++Count).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class Empty_collection
{
    [Test]
    public void Has_size_of_0()
        => ImmutableCollection.Empty.Should().BeEmpty();

    [Test]
    public void Contains_no_items()
        => ImmutableCollection.Empty.Should().BeEquivalentTo(Array.Empty<object>());

    [Test]
    public void Add_item_increases_the_size()
        => ImmutableCollection.Empty.Add(new Dummy()).Should().ContainSingle();
}

public class Non_empty_collection
{
    [Test]
    public void Add_item_increases_the_size()
        => ImmutableCollection.Empty
            .Add(new Dummy())
            .Add(new Other())
        .Should().HaveCount(2);
}

public class Add
{
    [Test]
    public void Event_increases_the_size()
        => ImmutableCollection.Empty.Add(new Dummy()).Should().ContainSingle();

    [Test]
    public void Null_item_has_no_effect()
        => ImmutableCollection.Empty.Add(null!).Should().BeEmpty();

    [Test]
    public void Null_items_are_ignored()
        => ImmutableCollection.Empty.AddRange(null!, 1, null!, 2, 3, null!, 4)
        .Should().BeEquivalentTo([1, 2, 3, 4]);

    [Test]
    public void String_is_not_considered_an_collection()
        => ImmutableCollection.Empty.Add("some string").Should().ContainSingle();

    [Test]
    public void Does_not_effect_shared_subs()
    {
        var parent = ImmutableCollection.Empty.AddRange(1, 2);
        var first = parent.AddRange(4, 8, 16);
        var second = parent.AddRange(3, 4, 5);

        parent.Should().BeEquivalentTo([1, 2]);
        first.Should().BeEquivalentTo([1, 2, 4, 8, 16]);
        second.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }

    [Test]
    public void with_plus_operator()
    {
        var collection = ImmutableCollection.Empty;
        collection += 3;
        collection += new[] { 6, 9 };
        collection.Should().BeEquivalentTo([3, 6, 9]);
    }
}

public class If_not_true
{
    [Test]
    public void Then_not_executed()
    {
        var events = ImmutableCollection.Empty
            .If(Help.NotTrue)
            .Then(Help.FailingCreation);

        events.Should().BeEmpty();
    }

    [Test]
    public void Else_increases_size()
    {
        var events = ImmutableCollection.Empty
            .If(Help.NotTrue)
                .Then(Help.FailingCreation)
            .Else(() => new Dummy());

        events.Should().ContainSingle();
    }
}

public class If_false
{
    [Test]
    public void Then_not_executed()
    {
        var events = ImmutableCollection.Empty
            .If(false)
            .Then(Help.FailingCreation);

        events.Should().BeEmpty();
    }

    [Test]
    public void Then_increases_size()
    {
        var events = ImmutableCollection.Empty
            .If(false)
                .Then(Help.FailingCreation)
            .Else(() => new Dummy());

        events.Should().ContainSingle();
    }

    [Test]
    public void Else_if_true_increases_size()
    {
        var events = ImmutableCollection.Empty
            .If(false)
                .Then(Help.FailingCreation)
            .ElseIf(true)
                .Then(() => new Dummy())
            .Else(Help.FailingCreation);

        events.Should().ContainSingle();
    }

    [Test]
    public void Else_if_false_not_executed()
    {
        var events = ImmutableCollection.Empty
            .If(false)
                .Then(Help.FailingCreation)
            .ElseIf(false)
                .Then(Help.FailingCreation)
            .Else(() => new Dummy());

        events.Should().ContainSingle();
    }

}

public class If_true
{
    [Test]
    public void Then_increases_size()
    {
        var events = ImmutableCollection.Empty
            .If(true)
            .Then(() => new Dummy());

        events.Should().ContainSingle();
    }

    [Test]
    public void Else_not_executed()
    {
        var events = ImmutableCollection.Empty
            .If(true)
                .Then(() => new Dummy())
            .Else(Help.FailingCreation);

        events.Should().ContainSingle();
    }

    [Test]
    public void Else_if_not_executed()
    {
        var events = ImmutableCollection.Empty
            .If(true)
                .Then(() => new Dummy())
            .ElseIf(true)
                .Then(Help.FailingCreation)
            .Else(Help.FailingCreation);

        events.Should().ContainSingle();
    }
}

public class Then_
{
    public class if_true
    {
        private readonly Then IfTrue = ImmutableCollection
            .Empty.If(true)
                .Then(() => new Dummy());

        [Test]
        public void Adds_item() 
            => IfTrue.Add(new Dummy())
                .Should().HaveCount(2);

        [Test]
        public void Adds_range_of_items()
            => IfTrue.AddRange(new Dummy(), new Dummy())
                .Should().HaveCount(3);

        [Test]
        public void If_true_adds()
            => IfTrue.If(true)
                .Then(() => new Dummy())
            .Should().HaveCount(2);

        [Test]
        public void If_false_does_not_add()
            => IfTrue.If(false)
                .Then(() => new Dummy())
            .Should().ContainSingle();
    }

    public class if_false
    {
        private readonly Then IfFalse = ImmutableCollection
          .Empty.If(false)
              .Then(() => new Dummy());

        [Test]
        public void Adds_item()
            => IfFalse.Add(new Dummy())
                .Should().ContainSingle();

        [Test]
        public void Adds_range_of_items()
            => IfFalse.AddRange(new Dummy(), new Dummy())
                .Should().HaveCount(2);

        [Test]
        public void If_true_adds()
            => IfFalse.If(true)
                .Then(() => new Dummy())
            .Should().ContainSingle();

        [Test]
        public void If_false_does_not_add()
            => IfFalse.If(false)
                .Then(() => new Dummy())
            .Should().BeEmpty();
    }
}

public class Enumerables
{
    [Test]
    public void are_added_as_fixed_list()
    {
        var events = ImmutableCollection.Empty.Add(new Dummies());
        events.Should().ContainSingle(because: "the initial count is 1.");
        events.Should().ContainSingle(because: "calling it multiple times should not change a thing.");
    }
}
