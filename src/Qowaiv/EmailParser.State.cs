using Qowaiv.Text;

namespace Qowaiv
{
    internal static partial class EmailParser
    {
        private ref struct State
        {
            public State(string str)
            {
                Input = str.Buffer().Trim();
                Buffer = CharBuffer.Empty(EmailAddress.MaxLength);
                Result = CharBuffer.Empty(EmailAddress.MaxLength);
            }

            public readonly CharBuffer Input;
            public readonly CharBuffer Buffer;
            public readonly CharBuffer Result;
            public override string ToString() => $"In: {Input}, Buf: {Buffer}, Res:{Result}";

            /// <summary>Gets the first <see cref="char"/> of the buffer, and removes it.</summary>
            public char Next()
            {
                var ch = Input.First();
                Input.RemoveFromStart(1);
                return ch;
            }

            public char Prev()
            {
                var ch = Input.Last();
                Input.RemoveFromEnd(1);
                return ch;
            }

            public char NextNoComment()
            {
                var ch = Next();
                if (!ch.IsCommentStart()) { return ch; }

                while (Input.NotEmpty() && !ch.IsCommentEnd())
                {
                    ch = Next();
                    if (ch.IsCommentStart()) { return default; }
                }
                return NextNoComment();
            }

            public State Invalid()
            {
                Input.Clear();
                Buffer.Clear();
                Result.Clear();
                return this;
            }

            public string Parsed() => Result.IsEmpty() ? null : Result.ToString();
        }
    }
}
