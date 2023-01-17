namespace Qowaiv;

internal static partial class EmailParser
{
    private ref struct State
    {
        public State(string? str)
        {
            Input = str.Buffer().Trim();
            Buffer = CharBuffer.Empty(EmailAddress.MaxLength);
            Result = CharBuffer.Empty(EmailAddress.MaxLength);
        }

        public readonly CharBuffer Input;
        public readonly CharBuffer Buffer;
        public readonly CharBuffer Result;

        [Pure]
        public override string ToString() => $"In: {Input}, Buf: {Buffer}, Res:{Result}";

        /// <summary>Gets the first <see cref="char"/> of the buffer, and removes it.</summary>
        [Impure]
        public char Next()
        {
            var ch = Input.First();
            Input.RemoveFromStart(1);
            return ch;
        }

        [Impure]
        public char Prev()
        {
            var ch = Input.Last();
            Input.RemoveFromEnd(1);
            return ch;
        }

        [Impure]
        public char NextNoComment()
        {
            var ch = Next();
            if (!ch.IsCommentStart()) { return ch; }

            while (Input.NotEmpty() && !ch.IsCommentEnd())
            {
                ch = Next();
                if (ch.IsCommentStart()) { return default; }
            }
            return Input.Length == 0
                ? default 
                : NextNoComment();
        }

        [Impure]
        public State Invalid()
        {
            Input.Clear();
            Buffer.Clear();
            Result.Clear();
            return this;
        }

        [Pure]
        public string? Parsed() => Result.IsEmpty() ? null : Result.ToString();
    }
}
