using System.IO;

namespace Specs.TestTools;

internal static class Embedded
{
    public static Stream Stream(string path)
        => typeof(Embedded).Assembly.GetManifestResourceStream(path.Replace('\\', '.').Replace('/', '.'))
        ?? throw new FileNotFoundException("Could not load the embedded resource.", path);
}
