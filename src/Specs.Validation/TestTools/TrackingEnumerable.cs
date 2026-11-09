namespace Specs.TestTools;

/// <summary>An <see cref="IEnumerable{T}"/> (not an <see cref="ICollection"/>) that tracks
/// how its enumerator is fetched, advanced and disposed.</summary>
internal sealed class TrackingEnumerable(params int[] items) : IEnumerable<int>
{
    private readonly int[] items = items;

    /// <summary>The number of times <see cref="IEnumerator.MoveNext"/> returned true.</summary>
    public int Fetched { get; private set; }

    /// <summary>Indicates whether the enumerator has been disposed.</summary>
    public bool Disposed { get; private set; }

    public IEnumerator<int> GetEnumerator() => new TrackingEnumerator(this);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class TrackingEnumerator(TrackingEnumerable owner) : IEnumerator<int>
    {
        private int index = -1;

        public int Current => owner.items[index];

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (++index < owner.items.Length)
            {
                owner.Fetched++;
                return true;
            }
            return false;
        }

        public void Reset() => throw new NotSupportedException();

        public void Dispose() => owner.Disposed = true;
    }
}
