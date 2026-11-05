namespace TestTools.IO.Temporary_directory_specs;

public class Implicitly_converts
{
    [Test]
    public void To_directory_info_from_instance()
    {
        using var dir = new TemporaryDirectory();
        DirectoryInfo casted = dir;
        casted.Should().NotBeNull();
    }

    [Test]
    public void To_directory_info_from_null()
    {
        TemporaryDirectory? dir = null;
        DirectoryInfo? casted = dir;
        casted.Should().BeNull();
    }
}

public class Exists
{
    [Test]
    public void within_scope()
    {
        using var temp = new TemporaryDirectory();
        DirectoryInfo dir = temp;
        dir.Exists.Should().BeTrue();
    }

    [Test]
    public void not_outside_scope()
    {
        DirectoryInfo? dir = null;
        using (var temp = new TemporaryDirectory())
        {
            dir = temp;
        }
        dir.Exists.Should().BeFalse();
    }
}

public class Has_debugger_experience
{
    [Test]
    public void via_directory()
    {
        using var temp = new TemporaryDirectory();
        DirectoryInfo dir = temp;
        temp.ToString().Should().Be(dir.ToString());
    }
}
