using System.Diagnostics;

namespace HelloCommander.BL.Services;

public class TerminalProcessService : ITerminalProcessService
{
    private readonly HcRuntimePlatformInfo _hcRuntimePlatformInfo;

    public TerminalProcessService(IHcRuntimePlatform hcRuntimePlatform)
    {
        _hcRuntimePlatformInfo = hcRuntimePlatform.GetRuntimeInfo();
    }

    public async Task ExecuteAsync(string cmd, string workingDirectory)
    {
        switch (_hcRuntimePlatformInfo.OperatingSystem)
        {
            case HcOperatingSystemType.OSX:
                await BashExecuteAsync(cmd, workingDirectory);
                break;
            case HcOperatingSystemType.WinNT:
                await BatExecuteAsync(cmd, workingDirectory);
                break;
        }
    }

    public void OpenTerminal(string workingDirectory)
    {
        switch (_hcRuntimePlatformInfo.OperatingSystem)
        {
            case HcOperatingSystemType.OSX:
                OpenTerminal("/bin/bash", workingDirectory);
                break;
            case HcOperatingSystemType.WinNT:
                OpenTerminal("cmd.exe", workingDirectory);
                break;
        }
    }

    private async Task BatExecuteAsync(string cmd, string workingDirectory)
    {
        await StartProcessAsync("cmd.exe", $"/c {cmd}", workingDirectory);
    }

    private async Task BashExecuteAsync(string cmd, string workingDirectory)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        await StartProcessAsync("/bin/bash", $"-c \"{escapedArgs}\"", workingDirectory);
    }

    private void OpenTerminal(string runner, string workingDirectory)
    {
        var process = new Process();
        process.StartInfo = new ProcessStartInfo(runner)
        {
            UseShellExecute = true,
            CreateNoWindow = false,
            WorkingDirectory = workingDirectory,
        };
        process.Start();
    }

    private Task StartProcessAsync(string runner, string args, string workingDirectory)
    {
        var process = new Process();
        process.StartInfo = new ProcessStartInfo(runner, args)
        {
            UseShellExecute = true,
            CreateNoWindow = false,
            WorkingDirectory = workingDirectory,
        };
        process.Start();
        return Task.CompletedTask;
    }
}
