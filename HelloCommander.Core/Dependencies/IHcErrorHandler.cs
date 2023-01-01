using HelloCommander.Core.Exceptions.Base;

namespace HelloCommander.Core.Dependencies;

public interface IHcErrorHandler
{
    Task HandleUserErrorAsync(string command, HcExceptionBase exception);
    Task HandleSystemErrorAsync(string command, Exception exception);
}
