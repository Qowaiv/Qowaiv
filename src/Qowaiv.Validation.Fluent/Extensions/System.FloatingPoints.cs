namespace System;

internal static class FloatingPointExtensions
{
    extension(double d)
    {
        [Pure]
        public bool IsFinite()
#if NETSTANDARD
            => !double.IsNaN(d) && !double.IsInfinity(d);
#else
            => double.IsFinite(d);
#endif
    }

    extension(float f)
    {
        [Pure]
        public bool IsFinite()
#if NETSTANDARD
            => !float.IsNaN(f) && !float.IsInfinity(f);
#else
            => float.IsFinite(f);
#endif
    }
}
