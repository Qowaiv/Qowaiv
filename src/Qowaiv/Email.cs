namespace Qowaiv;

public static class Email
{
    private const int NoMatch = -1;
    private const int MaxLength = 254;

    public static string? Parse(ReadOnlySpan<char> str)
    {
        Span<char> buffer = stackalloc char[MaxLength];
        var parser = new Parser(str.Trim(), buffer, 0);
        var parsed = parser.Email();

        return parsed.Result;
    }

    private ref struct Parser
    {
        private readonly ReadOnlySpan<char> Input;
        private readonly Span<char> Buffer;
        private readonly int Length;

        private bool Success => Length != NoMatch;

        public string? Result
            => Success
            ? Buffer[..Length].ToString()
            : null;

        public Parser(ReadOnlySpan<char> input, Span<char> result, int length)
        {
            Input = input;
            Buffer = result;
            Length = length;
        }

        [Pure]
        public Parser Email()
            => DisplayName()
            .MailTo()
            .Local()
            .Domain();

        [Pure]
        private Parser DisplayName()
        {
            if (Input[0] == '"')
            {
                return None();
            }
            else if (Input[^1] == '>')
            {
                var lt = Input.LastIndexOf('<');
                return lt == NoMatch
                    ? None()
                    : new(Input[(lt + 1)..^1], Buffer, Length);
            }
            else if (Input[^1] == ')')
            {
                var op = Input.LastIndexOf('(');

                return op == NoMatch
                    || Input[(op + 1)..^1].Contains(')')
                    ? None()
                    : new(Input[..op].TrimEnd(), Buffer, Length);
            }
            else
            {
                return this;
            }
        }

        [Pure]
        private Parser MailTo()
            => Input.StartsWith("MailTo:".AsSpan(), StringComparison.OrdinalIgnoreCase)
            ? new(Input[7..], Buffer, Length)
            : this;

        [Pure]
        private Parser Local()
            => RegularLocal() is { Success: true } local
            ? local
            : QuotedLocal();

        [Pure]
        private Parser RegularLocal()
        {
            if (Length == NoMatch) return this;

            var (ch, index) = Next(-1);
            var length = 0;
            var comment = false;
            char prev = default;

            while (index < Input.Length && length < MaxLength)
            {
               
                if (comment)
                {
                    // No nesting
                    if (ch == '(') return None();
                    if (ch == ')') comment = false;
                }
                else
                {
                    if (ch == '(') comment = true;
                    // . at start or two.
                    else if (ch == '.' && (length == 0 || prev == '.')) return None();
                    else if (ch == '@')
                    {
                        Buffer[length++] = ch;
                        return length == 0
                            || prev == '.'
                            ? None()
                            : new(Input[(index + 1)..], Buffer, length);
                    }
                    else if (length < 64 && EmailParser.IsLocal(ch))
                    {
                        Buffer[length++] = ch;
                        prev = ch;
                    }
                    else return None();
                }
            }
            return None();
        }

        [Pure]
        private Parser QuotedLocal()
        {
            if (Length == NoMatch) return this;

            return None();
        }

        [Pure]
        private Parser Domain()
            => RegularDomain() is { Success: true } domain
            ? domain
            : IpDomain();

        [Pure]
        private Parser RegularDomain()
        {
            if (Length == NoMatch) return this;

            var length = Length;
            var index = 0;
            var comment = false;
            char prev = default;

            while (index < Input.Length && length < MaxLength)
            {
                var ch = Input[index++];
                if (comment)
                {
                    // No nesting
                    if (ch == '(') return None();
                    if (ch == ')') comment = false;
                }
                else
                {
                    if (ch == '(') comment = true;
                    // . at start or two.
                    else if (ch == '.' && (length == 0 || prev == '.')) return None();
                    else if (length < 64 && EmailParser.IsDomain(ch))
                    {
                        Buffer[length++] = ch;
                        prev = ch;
                    }
                    else return None();
                }
            }
            return index == Input.Length
                ? new(Input, Buffer, length)
                : None();
        }

        [Pure]
        private Parser IpDomain()
        {
            return this;
        }

        [Pure]
        private Parser None() => new(Input, Buffer, NoMatch);

        [Pure]
        private (char, int) Next(int index)
        {
            var comment = false;

            while (++index < Input.Length)
            {
                var ch = Input[index];
                switch (ch)
                {
                    case '(':
                        if (comment) return (default, NoMatch);
                        else comment = true;
                        break;

                    case ')': comment = false; break;
                    default: if (!comment) return (ch, index); break;
                }
            }
            return (default, NoMatch);
        }

        [Pure]
        public override string ToString()
            => Success
            ? $"Input = {Input.ToString()}, Result = {Buffer[..Length].ToString()}"
            : $"Input = {Input.ToString()}, Result = {Buffer.ToString()}, Success = false";
    }
}
