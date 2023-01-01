namespace HelloCommander.Core.Dependencies;

public interface ITerminalProcessService
{
    Task ExecuteAsync(string cmd, string workingDirectory);
    void OpenTerminal(string workingDirectory);
}
