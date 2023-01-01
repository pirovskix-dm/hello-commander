using Avalonia.Threading;
using HelloCommander.Core.Dependencies;

namespace HelloCommander.HcApp.Dependencies;

public class SynchronizationHelper : ISynchronizationHelper
{
    public Task InvokeAsync(Action action)
    {
        return Dispatcher.UIThread.InvokeAsync(action, DispatcherPriority.Background);
    }

    public void Post(Action action)
    {
        Dispatcher.UIThread.Post(action, DispatcherPriority.Background);
    }
}
