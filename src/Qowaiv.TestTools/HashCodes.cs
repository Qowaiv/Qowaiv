namespace Qowaiv.TestTools;

/// <summary>À collection of values which hash codes can be asserted on.</summary>
[DebuggerDisplay("Count = {Values.Length}")]
[StackTraceHidden]
public sealed class HashCodes
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly object[] Values;

    private HashCodes(object[] values) => Values = values;

    /// <summary>The (unique) values which hash codes to assert on.</summary>
    [Pure]
    public static HashCodes For<T>(params T[] values) where T : notnull
        => new([.. Guard.HasAny(values)]);

    /// <summary>Assertions on <see cref="HashCodes"/>.</summary>
    [Pure]
    [DebuggerStepThrough]
    public Assertions Should() => new(this);

    /// <summary>Assertions on <see cref="HashCodes" />.</summary>
    public sealed class Assertions(HashCodes subject)
    {
        /// <summary>The subject(s) under test.</summary>
        public HashCodes Subject { get; } = Guard.NotNull(subject);

        /// <summary>Asserts hash codes to be conisistent.</summary>
        [DebuggerStepThrough]
        public void BeConsistent()
        {
            var values = Subject.Values;

            // We can not rely on a hash codes here: we have to test their correctness.
            for (var i = 1; i < values.Length; i++)
                for (var j = 0; j < i; j++)
                    if (Equals(values[i], values[j]))
                        throw new AssertionFailed($"Expected all values to be unique, but Values[{j}] and Values[{i}] where not.");

            int[] hash = [.. values.Select(v => v.GetHashCode())];

            var inconsistent = 0;

            var sb = new StringBuilder();

            sb.AppendLine("Expected all hashes to be consistent but");
            for (var i = 0; i < values.Length; i++)
            {
                var test = values[i].GetHashCode();
                if (hash[i] != test)
                {
                    sb.AppendLine($"Values[{i}] = {values[i]}");
                    inconsistent++;
                }
            }
            sb.Append($"{(inconsistent is 1 ? "was" : "were")} not.");

            if (inconsistent > 0) throw new AssertionFailed(sb.ToString());
        }
    }
}
