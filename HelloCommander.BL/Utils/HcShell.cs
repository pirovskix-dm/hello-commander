using System.Diagnostics;

namespace HelloCommander.BL.Utils;

// https://loune.net/2017/06/running-shell-bash-commands-in-net-core/
internal static class HcShell
{
    internal static async Task<string> BashAsync(this string cmd, string workingDirectory)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var result = await RunAsync("/bin/bash", $"-c \"{escapedArgs}\"", workingDirectory);
        return result;
    }

    internal static async Task<string> BatAsync(this string cmd, string workingDirectory)
    {
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var result = await RunAsync("cmd.exe", $"/c \"{escapedArgs}\"", workingDirectory);
        return result;
    }

    private static async Task<string> RunAsync(string filename, string arguments, string workingDirectory)
    {
        var process = new Process()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = filename,
                Arguments = arguments,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                WorkingDirectory = workingDirectory
            }
        };
        process.Start();
        var result = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();
        return result;
    }
}
