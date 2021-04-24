using Qowaiv.Text;

namespace Qowaiv
{
    /// <summary>
    /// 
    /// # Grammar
    /// mail  => local at domain
    /// local  => [^@]{1,64}@
    /// domain => .+}
    /// </summary>
    internal static class EmailParser2
    {
        /// <summary>The maximum length of the local part is 64.</summary>
        private const int LocalMaxLength = 64;

        public static string Parse(string str)
            => new State(str)
                .Local()
                .Domain()
                .Parsed();
    
        private static State Local(this State state)
        {
            while (state.Input.NotEmpty() && state.Buffer.Length < LocalMaxLength)
            {
                var ch = state.Input.Next();
                state.Buffer.Add(ch);

                if (state.Buffer.Last().At())
                {
                    state.Result.Add(state.Buffer);
                    state.Buffer.Clear();
                    return state;
                }
            }
            return state.Invalid();
        }

        private static State Domain(this State state)
        {
            while (state.Input.NotEmpty() && state.Buffer.Length + state.Result.Length < EmailAddress.MaxLength)
            {
                var ch = state.Input.Next();
                state.Buffer.Add(ch);
            }
            return state.Input.IsEmpty()
                ? state
                : state.Invalid();
        }

        private static bool At(this char ch) => ch == '@';
        private static bool Digit(this char ch) => ch >= '0' && ch <= '9';
        private static bool Letter(this char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        private static bool Underscore(this char ch) => ch == '_';
        private static bool Dot(this char ch) => ch == '.';
        private static bool Dash(this char ch) => ch == '-';
        private static bool NonASCII(this char ch) => ch > 127;

        /// <summary>Internal state.</summary>
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
            public override string ToString() => $"Buffer: {Input}, Result:{Result}";

            public State Invalid()
            {
                Input.Clear();
                Result.Clear();
                return this;
            }

            public string Parsed() => Result.IsEmpty() ? null : Result.ToString();
        }
    }
}
