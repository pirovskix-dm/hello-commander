namespace HelloCommander.Core.Exceptions.Base;

public abstract class HcExceptionBase : Exception
{
    protected HcExceptionBase()
    {
    }

    protected HcExceptionBase(string message) : base(message)
    {
    }
}
