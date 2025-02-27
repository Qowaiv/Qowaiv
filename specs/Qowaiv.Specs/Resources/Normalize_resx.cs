using Qowaiv.TestTools.Resx;
using System.Xml;

namespace Resources.Normalize_resx;

public class All
{
    private static readonly DirectoryInfo Root = new("../../../../../src");

    public static FileInfo[] Resources => Root.GetFiles("*.resx", SearchOption.AllDirectories);

    [TestCaseSource(nameof(Resources))]
    public void IsOrded(FileInfo file)
    {
        using var read = file.OpenRead();
        var resx = XResourceFile.Load(read);

        read.Close();

        resx.Data.Sort();

        using var write = XmlWriter.Create(file.FullName, new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\r\n",
            OmitXmlDeclaration = false,
            Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false),
        });

        resx.Save(write);

    }
}
