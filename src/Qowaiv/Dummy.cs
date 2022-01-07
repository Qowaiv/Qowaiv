namespace Qowaiv;

public class Dummy : IDisposable
{
    public int X() => throw new NullReferenceException();


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
