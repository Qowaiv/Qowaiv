namespace Qowaiv.Text
{
    internal static class CharrBufferExtensions
    {
        public static CharBuffer Buffer(this string str)
           => str is null
           ? CharBuffer.Empty(0)
           : new CharBuffer(str);
    }
}
