namespace System
{
    internal static class FuncExtensions
    {
        public static Func<object, TOut> ToNongenericInput<TIn, TOut>(this Func<TIn, TOut> func)
        {
            return x => func((TIn)x);
        }
    }
}
