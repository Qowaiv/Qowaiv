namespace Qowaiv.TestTools;

public static class QAssert
{
    /// <remarks>A cast can not be applied unless you assign it, therefore a <see cref="Func{TResult}"/>.</remarks>
    [DebuggerStepThrough]
    public static void InvalidCast<TSvo>(Func<TSvo> cast)
    {
        var x = Assert.Catch<InvalidCastException>(() => cast());
        StringAssert.IsMatch($"Cast from .+ to {typeof(TSvo)} is not valid.", x.Message);
    }
}
