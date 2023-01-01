using HelloCommander.Core.Commands;
using HelloCommander.Core.Exceptions.Base;

namespace HelloCommander.BL.Services;

public class HcCommandFactory : IHcCommandFactory
{
    private readonly IHcErrorHandler _hcErrorHandler;

    public HcCommandFactory(IHcErrorHandler hcErrorHandler)
    {
        _hcErrorHandler = hcErrorHandler;
    }

    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command)
    {
        return new AsyncDelegateCommand(name, command, _hcErrorHandler);
    }

    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command, Func<bool> canExecute)
    {
        return new AsyncDelegateCommand(name, command, _hcErrorHandler, canExecute);
    }

    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command, Func<HcExceptionBase, Task> errorHandler)
    {
        return new AsyncDelegateCommand(name, command, _hcErrorHandler, null, errorHandler);
    }

    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command, Func<bool> canExecute, Func<HcExceptionBase, Task> errorHandler)
    {
        return new AsyncDelegateCommand(name, command, _hcErrorHandler, canExecute, errorHandler);
    }

    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command)
    {
        return new AsyncDelegateCommand<T>(name, command, _hcErrorHandler);
    }

    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command, Func<T, bool> canExecute)
    {
        return new AsyncDelegateCommand<T>(name, command, _hcErrorHandler, canExecute);
    }

    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command, Func<HcExceptionBase, Task> errorHandler)
    {
        return new AsyncDelegateCommand<T>(name, command, _hcErrorHandler, null, errorHandler);
    }

    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command, Func<T, bool> canExecute, Func<HcExceptionBase, Task> errorHandler)
    {
        return new AsyncDelegateCommand<T>(name, command, _hcErrorHandler, canExecute, errorHandler);
    }
}
