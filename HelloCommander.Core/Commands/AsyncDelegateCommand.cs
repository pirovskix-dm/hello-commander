using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using HelloCommander.Core.Dependencies;
using HelloCommander.Core.Exceptions.Base;

namespace HelloCommander.Core.Commands;

public class AsyncDelegateUnit
{
}

public class AsyncDelegateCommand : AsyncDelegateCommand<AsyncDelegateUnit>
{
    public AsyncDelegateCommand(string name, Func<Task> command, IHcErrorHandler hcErrorHandler, Func<bool> canExecute = null, Func<HcExceptionBase, Task> exceptionHandler = null)
        : base(name, _ => command(), hcErrorHandler, _ => canExecute?.Invoke() ?? true, exceptionHandler)
    {
    }
}

public class AsyncDelegateCommand<T> : IAsyncCommand<T>
{
    public event EventHandler CanExecuteChanged;

    private bool _isExecuting;

    private readonly string _name;

    private readonly Func<T, Task> _command;

    private readonly Func<T, bool> _canExecute;

    private readonly Func<HcExceptionBase, Task> _errorHandler;

    private readonly IHcErrorHandler _hcErrorHandler;

    public AsyncDelegateCommand(string name, Func<T, Task> command, IHcErrorHandler hcErrorHandler, Func<T, bool> canExecute = null,
        Func<HcExceptionBase, Task> errorHandler = null)
    {
        _isExecuting = false;
        _name = name;
        _command = command;
        _canExecute = canExecute;
        _errorHandler = errorHandler;
        _hcErrorHandler = hcErrorHandler;
    }

    public bool CanExecute(object parameter)
    {
        if (typeof(T) == typeof(AsyncDelegateUnit))
        {
            return !_isExecuting && (_canExecute?.Invoke(default) ?? true);
        }

        return !_isExecuting && (_canExecute?.Invoke((T)parameter) ?? true);
    }

    public void Execute(object parameter)
    {
        ExecuteAsync(typeof(T) == typeof(AsyncDelegateUnit) ? default : (T)parameter).SafeFireAndForget();
    }

    public async Task ExecuteAsync(T parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                await _command.Invoke(parameter);
            }
            catch (HcExceptionBase ex)
            {
                await _hcErrorHandler.HandleUserErrorAsync(_name, ex);
                await _errorHandler(ex);
            }
            catch (Exception ex)
            {
                await _hcErrorHandler.HandleSystemErrorAsync(_name, ex);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
