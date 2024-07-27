namespace Qowaiv;

internal static partial class EmailParser
{
    private readonly struct Context(CharSpan input, Chars output)
    {
        public readonly CharSpan In = input;
        public readonly Chars Out = output;

        [Pure]
        public Context Next() => new(In.Next(), Out);

        [Pure]
        public Context? NextNoComment()
        {
            if (In.IsEmpty) { return null; }

            var @in = In;

            if (@in is { NotEmpty: true, First: '(' })
            {
                while (++@in is { IsEmpty: false })
                {
                    if (@in.First == '(')
                    {
                        return null;
                    }
                    else if (@in.First == ')')
                    {
                        return new(@in.Next(), Out);
                    }
                }
                return null;
            }
            return this;
        }

        [Pure]
        public override string ToString() => $"In: {In}, Out: {Out}";

        public static Context operator +(Context c, char ch) => new(c.In, c.Out + ch);
    }
}
