namespace Qowaiv;

public class Dummy : IDisposable
{
    public void X() => throw new NullReferenceException();


    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
