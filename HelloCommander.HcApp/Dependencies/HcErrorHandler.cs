using HelloCommander.Core.Dependencies;
using HelloCommander.Core.Exceptions.Base;

namespace HelloCommander.HcApp.Dependencies;

public class HcErrorHandler : IHcErrorHandler
{
    private readonly IUserInteractionHelper _userInteractionHelper;

    public HcErrorHandler(IUserInteractionHelper userInteractionHelper)
    {
        _userInteractionHelper = userInteractionHelper;
    }

    public Task HandleSystemErrorAsync(string command, Exception exception)
    {
        var actualException = exception.InnerException?.InnerException ?? exception.InnerException ?? exception;
        var log = $"{exception.GetType().FullName} -> {actualException.GetType().FullName}{Environment.NewLine}";
        log += $"{command} error. {actualException.Message}:{Environment.NewLine}{exception.StackTrace}";
        Console.WriteLine(log);
        return _userInteractionHelper.ShowErrorMessage($"{command} failure. {actualException.Message}");
    }

    public Task HandleUserErrorAsync(string command, HcExceptionBase exception)
    {
        return _userInteractionHelper.ShowErrorMessage($"{command} failure. {exception.Message}");
    }
}
