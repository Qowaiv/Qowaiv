namespace System.Collections.Enumerable;

internal static class QowaivEnumerableExtensions
{
    extension(IEnumerable enumerable)
    {
        [Pure]
        public bool Any()
        {
            var enumerator = enumerable.GetEnumerator();
            try
            {
                return enumerator.MoveNext();
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        [Pure]
        public int Count()
        {
            var count = 0;
            var counter = enumerable.GetEnumerator();
            try
            {
                while (counter.MoveNext())
                    count++;

                return count;
            }
            finally
            {
                (counter as IDisposable)?.Dispose();
            }
        }
    }
}
