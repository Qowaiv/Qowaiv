namespace TestTools.IO.FileLock_specs;

public class Locks
{
    [Test]
    public void file_for_specified_scope()
    {
        using var dir = new TemporaryDirectory();

        var file = dir.CreateFile("test.txt");
        
        using (var writer = new StreamWriter(file.FullName, false))
        {
            writer.Write("Unit Test");
        }

        using (var @lock = file.Lock())
        {
            file.Invoking(ReadContent).Should().Throw<IOException>();
        }

        ReadContent(file).Should().Be("Unit Test");
    }

    private static string ReadContent(FileInfo file)
    {
        using var reader = file.OpenText();
        return reader.ReadToEnd();
    }
}
