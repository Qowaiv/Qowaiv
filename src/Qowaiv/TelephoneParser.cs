using Qowaiv.Text;

namespace Qowaiv
{
    internal static class TelephoneParser
    {
        public static string Parse(string str)
            => new State(str)
            .International()
            .Other()
            .Parsed();

        private static State International(this State state)
        {
            var matches = false;
            if (state.In.StartsWith("+"))
            {
                state.Out.Add('+');
                state.In.Next();
                matches = true;
            }
            else if (state.In.StartsWith("00"))
            {
                state.Out.Add('+');
                state.In.Next(2);
                matches = true;
            }

            if (matches)
            {
                // international should be followed by a non-zero digit.
                if (!state.In.IsDigit() || state.In.First() == '0')
                {
                    return state.Invalid();
                }

                while (state.In.IsDigit())
                {
                    state.Out.Add(state.In.First());
                    state.In.Next();
                }
                while (state.In.IsMarkup())
                {
                    state.In.Next();
                }

                // remove optional region prefix on international.
                if (state.In.StartsWith("(0)") ||
                    state.In.StartsWith("[0]"))
                {
                    state.In.Next(3);
                }
            }
            return state;
        }

        private static State Other(this State state)
        {
            while (state.In.NotEmpty())
            {
                if (state.In.IsDigit())
                {
                    state.Out.Add(state.In.First());
                }
                else if (!state.In.IsMarkup())
                {
                    return state.Invalid();
                }
                state.In.Next();
            }
            return state;
        }

        private ref struct State
        {
            public readonly CharBuffer In;
            public readonly CharBuffer Out;

            public State(string str)
            {
                In = str.Buffer().Trim();
                Out = CharBuffer.Empty(In.Length + 1);
            }

            public State Invalid()
            {
                In.Clear();
                Out.Clear();
                return this;
            }

            public string Parsed() => Out.Length < 3 ? null : Out.ToString();
        }

        private static CharBuffer Next(this CharBuffer buffer, int next = 1)
            => buffer.RemoveFromStart(next);

        private static bool IsDigit(this CharBuffer buffer)
            => buffer.NotEmpty()
            && buffer.First() >= '0'
            && buffer.First() <= '9';

        private static bool IsMarkup(this CharBuffer buffer)
            => buffer.NotEmpty()
            && CharBuffer.IsMarkup(buffer.First());
    }
}
