using HelloCommander.Core.Exceptions.Base;

namespace HelloCommander.Core.Exceptions;

public class HcForbiddenException : HcExceptionBase
{
    public HcForbiddenException(string message) : base(message)
    {
    }
}
