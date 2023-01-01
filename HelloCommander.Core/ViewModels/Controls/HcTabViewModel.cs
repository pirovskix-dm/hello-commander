using HelloCommander.Core.Dependencies;
using HelloCommander.Core.Utils;
using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.ViewModels.Controls;

public class HcTabViewModel : ViewModelBase, IAsyncInitializableComponent<Guid, string>
{
    public Guid Id { get; set; }

    public string Name => TabItemsContext.CurrentDirectory?.Name ?? string.Empty;

    public HcTabTerminalViewModel TabTerminalContext { get; set; }
    public HcTabItemsViewModel TabItemsContext { get; set; }


    public AsyncDelegateCommand MoveBackCommand { get; set; }
    public AsyncDelegateCommand MoveForwardCommand { get; set; }
    public AsyncDelegateCommand ReloadCommand { get; set; }

    private readonly INavigationHistory _navigationHistory;
    private readonly ISaveStateService _saveStateService;

    public HcTabViewModel(
        HcTabTerminalViewModel tabTerminalViewModel,
        HcTabItemsViewModel tabItemsViewModel,
        INavigationHistory navigationHistory,
        ISaveStateService saveStateService,
        IHcCommandFactory hcCommandFactory)
    {
        _navigationHistory = navigationHistory;
        _saveStateService = saveStateService;

        _navigationHistory.Changed += OnNavigationHistoryChanged;

        MoveBackCommand = hcCommandFactory.CreateAsyncCommand(LanguageResources.MoveBackCommand, OnMoveBack, MoveBackEnabled);
        MoveForwardCommand = hcCommandFactory.CreateAsyncCommand(LanguageResources.MoveForwardCommand, OnMoveForward, MoveForwardEnabled);
        ReloadCommand = hcCommandFactory.CreateAsyncCommand(LanguageResources.ReloadCommand, OnReloadAsync);

        TabItemsContext = tabItemsViewModel;
        TabTerminalContext = tabTerminalViewModel;

        TabTerminalContext.OnNavigate += OnTerminalNavigate;
        TabItemsContext.OnPathOpen += OnPathOpen;
    }

    private async Task OnPathOpen(IHcPath hcPath)
    {
        TabTerminalContext.UpdateWorkingDirectory(hcPath.Path);
        _navigationHistory.Add(hcPath); // TODO: don't add if opened by history
        await _saveStateService.UpdateTabAsync(Id, hcPath.Path);
    }

    private async Task OnTerminalNavigate(string path)
    {
        if (path == "..")
        {
            await TabItemsContext.LoadParentAsync();
            return;
        }

        if (path.StartsWith(Path.DirectorySeparatorChar))
        {
            await TabItemsContext.LoadPathAsync(path);
            return;
        }

        await TabItemsContext.LoadLocalFolder(path);
    }

    public async Task InitializeAsync(Guid id, string path)
    {
        Id = id;

        if (string.IsNullOrWhiteSpace(path))
        {
            TabItemsContext.LoadRoot();
        }
        else
        {
            await TabItemsContext.LoadPathAsync(path);
        }
    }

    private Task OnReloadAsync()
    {
        return TabItemsContext.LoadPathAsync(_navigationHistory.Current);
    }

    private void OnNavigationHistoryChanged()
    {
        MoveBackCommand.RaiseCanExecuteChanged();
        MoveForwardCommand.RaiseCanExecuteChanged();
    }

    private bool MoveForwardEnabled()
    {
        return _navigationHistory.IsMoveForwardEnabled;
    }

    private bool MoveBackEnabled()
    {
        return _navigationHistory.IsMoveBackEnabled;
    }

    private Task OnMoveForward()
    {
        return TabItemsContext.LoadPathAsync(_navigationHistory.MoveForward());
    }

    private Task OnMoveBack()
    {
        return TabItemsContext.LoadPathAsync(_navigationHistory.MoveBack());
    }
}
