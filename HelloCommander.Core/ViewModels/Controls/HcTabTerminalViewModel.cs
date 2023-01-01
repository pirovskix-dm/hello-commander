using HelloCommander.Core.Dependencies;
using HelloCommander.Core.Utils;
using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.ViewModels.Controls;

public class HcTabTerminalViewModel : ViewModelBase
{
    public event Func<string, Task> OnNavigate;

    public string WorkingDirectory { get; set; }
    public AsyncDelegateCommand<string> CommandTerminalEnterTextCommand { get; set; }

    private readonly ITerminalProcessService _terminalProcessService;

    public HcTabTerminalViewModel(
        IHcCommandFactory hcCommandFactory,
        ITerminalProcessService terminalProcessService)
    {
        _terminalProcessService = terminalProcessService;
        CommandTerminalEnterTextCommand = hcCommandFactory.CreateAsyncCommand<string>(LanguageResources.CommandTerminalEnterTextCommand, OnCommandTerminalEnterText);
    }

    public void UpdateWorkingDirectory(string path)
    {
        WorkingDirectory = path;
    }

    private async Task OnCommandTerminalEnterText(string cmd)
    {
        if (string.IsNullOrWhiteSpace(cmd))
        {
            return;
        }

        await _terminalProcessService.ExecuteAsync(cmd.Trim(), WorkingDirectory);
    }
}
