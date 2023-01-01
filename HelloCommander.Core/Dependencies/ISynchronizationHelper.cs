namespace HelloCommander.Core.Dependencies;

public interface ISynchronizationHelper
{
    Task InvokeAsync(Action action);
    void Post(Action action);
}
