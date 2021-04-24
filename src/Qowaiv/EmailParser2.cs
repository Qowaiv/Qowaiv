using Qowaiv.Text;

namespace Qowaiv
{
    /// <summary>
    /// 
    /// # Grammar
    /// mail  => [local] [domain]
    /// local  => [l] {1,65}
    /// l => @._- [a-z][0-9] [{}|/%$&#~!?*`'^=+] [non-ASCII]
    /// domain => .+
    /// </summary>
    internal static class EmailParser2
    {
        /// <summary>The maximum length of the local part is 64.</summary>
        private const int LocalMaxLength = 64;
        
        private const int NotFound = -1;

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
                if (ch.IsLocal())
                {
                    state.Buffer.Add(ch);

                    if (ch.IsAt())
                    {
                        state.Result.Add(state.Buffer);
                        state.Buffer.Clear();
                        return state;
                    }
                }
                else { return state.Invalid(); }
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

        private static bool IsAt(this char ch) => ch == '@';
        private static bool IsLocal(this char ch)
            => ch.IsDigit()
            || ch.IsLetter()
            || ch.IsUnderscore()
            || ch.IsDot()
            || ch.IsDash()
            || ch.IsNonASCII()
            || ch.IsLocalASCII();

        private static bool IsDigit(this char ch) => ch >= '0' && ch <= '9';
        private static bool IsLetter(this char ch) => (ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z');
        private static bool IsUnderscore(this char ch) => ch == '_';
        private static bool IsDot(this char ch) => ch == '.';
        private static bool IsDash(this char ch) => ch == '-';
        private static bool IsNonASCII(this char ch) => ch > 127;
        private static bool IsLocalASCII(this char ch) => "@{}|/%$&#~!?*`'^=+".IndexOf(ch) != NotFound;

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
