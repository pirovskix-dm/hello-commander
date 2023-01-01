using HelloCommander.Core.Exceptions.Base;

namespace HelloCommander.Core.Dependencies;

public interface IHcCommandFactory
{
    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command);
    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command, Func<bool> canExecute);
    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command, Func<HcExceptionBase, Task> errorHandler);
    public AsyncDelegateCommand CreateAsyncCommand(string name, Func<Task> command, Func<bool> canExecute, Func<HcExceptionBase, Task> errorHandler);
    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command);
    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command, Func<T, bool> canExecute);
    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command, Func<HcExceptionBase, Task> errorHandler);
    public AsyncDelegateCommand<T> CreateAsyncCommand<T>(string name, Func<T, Task> command, Func<T, bool> canExecute, Func<HcExceptionBase, Task> errorHandler);
}
