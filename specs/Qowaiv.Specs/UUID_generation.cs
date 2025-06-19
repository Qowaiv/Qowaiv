#if DEBUG
namespace UUID_generation;

public class Generates
{
    [Test]
    public void Lookup_Base64()
    {
        var bytes = new byte[256];
        for (var i = 0; i < bytes.Length; i++) { bytes[i] = 255; }

        for (var i = 'A'; i <= 'Z'; i++) { bytes[i] = (byte)(i - 'A'); }
        for (var i = 'a'; i <= 'z'; i++) { bytes[i] = (byte)(26 + i - 'a'); }
        for (var i = '0'; i <= '9'; i++) { bytes[i] = (byte)(52 + i - '0'); }

        bytes['-'] = 62; bytes['+'] = 62;
        bytes['_'] = 63; bytes['/'] = 63;

        var sb = new StringBuilder().Append('[');

        for (var i = 0; i < bytes.Length; i++)
        {
            if (i % 16 == 0) sb.AppendLine();
            var str = Convert.ToString(bytes[i], 16);
            sb.Append("0x").Append(str.Length == 1 ? "0" : "").Append(str).Append(", ");
        }

        sb.AppendLine().Append("];");

        Console.WriteLine(sb.Replace("0xff", "None"));

        bytes.Distinct().Should().HaveCount(65);
    }

    [Test]
    public void Lookup_Base32()
    {
        var values = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        var bytes = new byte[256];
        for (var i = 0; i < bytes.Length; i++) { bytes[i] = 255; }

        for (byte i = 0; i < 32; i++)
        {
            bytes[values[i]] = i;
            bytes[char.ToLowerInvariant(values[i])] = i;
        }

        bytes['0'] = bytes['O'];
        bytes['1'] = bytes['I'];

        var sb = new StringBuilder().Append('[');

        for (var i = 0; i < bytes.Length; i++)
        {
            if (i % 16 == 0) sb.AppendLine();
            var str = Convert.ToString(bytes[i], 16);
            sb.Append("0x").Append(str.Length == 1 ? "0" : "").Append(str).Append(", ");
        }

        sb.AppendLine().Append("];");

        Console.WriteLine(sb.Replace("0xff", "None"));

        bytes.Distinct().Should().HaveCount(33);
    }
}
#endif
