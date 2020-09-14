namespace Qowaiv.Text
{
    internal static class CharrBufferExtensions
    {
        public static CharBuffer Buffer(this string str)
           => str is null
           ? new CharBuffer(0)
           : new CharBuffer(str);
    }
}
