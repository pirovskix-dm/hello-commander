using System.Collections.ObjectModel;
using HelloCommander.Core.Dependencies;
using HelloCommander.Core.Exceptions.Base;
using HelloCommander.Core.Utils;
using HelloCommander.Core.ViewModels.Base;
using HelloCommander.Core.ViewModels.Controls.HcTabItems;

namespace HelloCommander.Core.ViewModels.Controls;

public class HcTabItemsViewModel : ViewModelBase
{
    public event Func<IHcPath, Task> OnPathOpen;

    private const int FILE_THREAD_CHUNK_SIZE = 10;

    public TabItemViewModelBase ParentDirectory { get; set; }
    public TabItemViewModelBase SelectedItem { get; set; }
    public TabItemViewModelBase CurrentDirectory { get; set; }
    public ObservableCollection<TabItemViewModelBase> Items { get; set; } = new();

    public AsyncDelegateCommand<TabItemViewModelBase> OpenCommand { get; set; }
    public AsyncDelegateCommand<TabItemViewModelBase> RenameTabItemCommand { get; set; }
    public AsyncDelegateCommand<TabItemViewModelBase> CopyTabItemCommand { get; set; }
    public AsyncDelegateCommand<TabItemViewModelBase> AddToBookmarksCommand { get; set; }
    public AsyncDelegateCommand<TabItemViewModelBase> DeleteTabItemCommand { get; set; }
    public AsyncDelegateCommand<TabItemViewModelBase> TerminalFromHereCommand { get; set; }
    public AsyncDelegateCommand<HcTabDirectoryViewModel> PasteTabItemCommand { get; set; }
    public AsyncDelegateCommand<TabItemViewModelBase> SetAsHomeCommand { get; set; }

    private CancellationTokenSource _cancellationTokenSource;
    private readonly Func<IHcDirectory, HcTabDirectoryViewModel> _hcTabDirectoryFactory;
    private readonly Func<IHcFile, HcTabFileViewModel> _hcTabFileFactory;
    private readonly Func<HcTabRootViewModel> _hcTabRootFactory;
    private readonly ISynchronizationHelper _synchronizationHelper;
    private readonly IUserInteractionHelper _userMessageHelper;
    private readonly IBookmarksService _bookmarksService;
    private readonly ISettingsService _settingsService;
    private readonly ITerminalProcessService _terminalProcessService;
    private readonly IHcFileService _hcFileService;

    public HcTabItemsViewModel(
        Func<IHcDirectory, HcTabDirectoryViewModel> hcTabDirectoryFactory,
        Func<IHcFile, HcTabFileViewModel> hcTabFileFactory,
        Func<HcTabRootViewModel> hcTabRootFactory,
        ISynchronizationHelper synchronizationHelper,
        IUserInteractionHelper userMessageHelper,
        IBookmarksService bookmarksService,
        IHcCommandFactory hcCommandFactory,
        ISettingsService settingsService,
        ITerminalProcessService terminalProcessService,
        IHcFileService hcFileService)
    {
        _hcTabDirectoryFactory = hcTabDirectoryFactory;
        _hcTabFileFactory = hcTabFileFactory;
        _hcTabRootFactory = hcTabRootFactory;
        _synchronizationHelper = synchronizationHelper;
        _userMessageHelper = userMessageHelper;
        _bookmarksService = bookmarksService;
        _settingsService = settingsService;
        _terminalProcessService = terminalProcessService;
        _hcFileService = hcFileService;

        _hcFileService.CopyStateChanged += OnFileCopyStateChanged;

        OpenCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.OpenFileCommand, OnOpenAsync, OnOpenErrorAsync);
        RenameTabItemCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.RenameTabItemCommand, OnRenameTabItemAsync);
        CopyTabItemCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.CopyTabItemCommand, OnCopyTabItemAsync);
        PasteTabItemCommand = hcCommandFactory.CreateAsyncCommand<HcTabDirectoryViewModel>(LanguageResources.PasteTabItemCommand, OnPastTabItemAsync, PasteEnabled);
        DeleteTabItemCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.DeleteTabItemCommand, OnDeleteTabItemAsync);
        TerminalFromHereCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.TerminalFromHereCommand, OnTerminalFromHere);
        AddToBookmarksCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.AddToBookmarksCommand, OnAddToBookmarksAsync);
        SetAsHomeCommand = hcCommandFactory.CreateAsyncCommand<TabItemViewModelBase>(LanguageResources.SetAsHomeCommand, OnSetAsHome);
    }

    public void StopLoadingItmes()
    {
        _cancellationTokenSource?.Cancel();
    }

    public async Task LoadParentAsync()
    {
        if (ParentDirectory != null)
        {
            await LoadPathAsync(ParentDirectory.FullName);
        }

        LoadRoot();
    }

    public Task LoadLocalFolder(string folder)
    {
        return LoadPathAsync(Path.Combine(CurrentDirectory.FullName, folder));
    }

    public Task LoadPathAsync(string path)
    {
        var hcPath = _hcFileService.GetPath(path);
        return LoadPathAsync(hcPath);
    }

    public async Task LoadPathAsync(IHcPath path)
    {
        switch (path)
        {
            case IHcDirectory directory:
                await LoadItemsAsync(directory);
                break;
            case IHcSystemRoot:
                LoadRoot();
                break;
            case IHcFile file:
                _hcFileService.RunFile(file);
                break;
        }
    }

    public void LoadRoot()
    {
        StopLoadingItmes();

        Items = new ObservableCollection<TabItemViewModelBase>();

        CurrentDirectory = _hcTabRootFactory.Invoke();

        var hcSystemRoot = _hcFileService.GetSystemRoot();
        foreach (var directory in hcSystemRoot.GetLogicalDrives())
        {
            Items.Add(_hcTabDirectoryFactory.Invoke(directory));
        }

        OnPathOpen?.Invoke(hcSystemRoot);
    }

    public async Task LoadItemsAsync(IHcDirectory hcDirectory)
    {
        StopLoadingItmes();
        _cancellationTokenSource = new CancellationTokenSource();
        await LoadItemsAsync(hcDirectory, _cancellationTokenSource.Token);
        OnPathOpen?.Invoke(hcDirectory);
    }

    private Task OnTerminalFromHere(TabItemViewModelBase tabItem)
    {
        _terminalProcessService.OpenTerminal(tabItem.Path);
        return Task.CompletedTask;
    }

    private async Task OnOpenAsync(TabItemViewModelBase tabItem)
    {
        switch (tabItem)
        {
            case HcTabRootViewModel:
                LoadRoot();
                break;
            case HcTabDirectoryViewModel:
                var directory = _hcFileService.GetDirectory(tabItem.FullName);
                await LoadItemsAsync(directory);
                break;
            case HcTabFileViewModel:
                var file = _hcFileService.GetFile(tabItem.FullName);
                _hcFileService.RunFile(file);
                break;
        }
    }

    private Task OnOpenErrorAsync(HcExceptionBase exception)
    {
        // TODO: _navigationHistory.Revert();
        return LoadParentAsync();
    }

    private bool PasteEnabled(HcTabDirectoryViewModel _)
    {
        return _hcFileService.HasFileToPaste;
    }

    private Task OnSetAsHome(TabItemViewModelBase tabItem)
    {
        return _settingsService.UpdateHomeDirectory(tabItem.FullName);
    }

    private async Task OnDeleteTabItemAsync(TabItemViewModelBase tabItem)
    {
        var replaceConfirmed = await _userMessageHelper.ShowYesNoMessageAsync($"{LanguageResources.ConfirmDelete}: {tabItem.Name}?");
        if (replaceConfirmed == HcYesNoMessageBoxResult.Yes)
        {
            await tabItem.Delete();
            Items.Remove(tabItem);
        }
    }

    private async Task OnAddToBookmarksAsync(TabItemViewModelBase tabItem)
    {
        await _bookmarksService.AddBookmarkAsync(tabItem);
    }

    private async Task OnPastTabItemAsync(HcTabDirectoryViewModel hcTabDirectoryViewModel)
    {
        var copiedItem = await _hcFileService.PastAsync(hcTabDirectoryViewModel.FullName, async path =>
        {
            var replaceConfirmed = await _userMessageHelper.ShowYesNoMessageAsync($"{LanguageResources.ConfirmFileReplace}: {path}");
            return replaceConfirmed == HcYesNoMessageBoxResult.Yes;
        });

        switch (copiedItem)
        {
            case IHcDirectory directory:
                Items.Add(_hcTabDirectoryFactory.Invoke(directory));
                break;
            case IHcFile file:
                Items.Add(_hcTabFileFactory.Invoke(file));
                break;
        }
    }

    private void OnFileCopyStateChanged()
    {
        PasteTabItemCommand.RaiseCanExecuteChanged();
    }

    private async Task OnCopyTabItemAsync(TabItemViewModelBase tabItem)
    {
        _hcFileService.Copy(tabItem is HcTabDirectoryViewModel, tabItem.FullName);
    }

    private async Task OnRenameTabItemAsync(TabItemViewModelBase tabItem)
    {
        var renameResult = await _userMessageHelper.EditTextWindowAsync(LanguageResources.FileRename, tabItem.Name);
        if (renameResult.IsConfirmed)
        {
            await tabItem.RenameAsync(renameResult.Text);
        }
    }

    private async Task LoadItemsAsync(IHcDirectory hcDirectory, CancellationToken ct)
    {
        if (ct.IsCancellationRequested)
        {
            return;
        }

        Items = new ObservableCollection<TabItemViewModelBase>();

        CurrentDirectory = _hcTabDirectoryFactory.Invoke(hcDirectory);

        if (hcDirectory.Parent != null)
        {
            ParentDirectory = _hcTabDirectoryFactory.Invoke(hcDirectory.Parent);
            ParentDirectory.Name = LanguageResources.BackFolderName;
            Items.Add(ParentDirectory);
        }
        else
        {
            ParentDirectory = null;
            var root = _hcTabRootFactory.Invoke();
            root.Name = LanguageResources.BackFolderName;
            Items.Add(root);
        }

        foreach (var chunkDirectory in hcDirectory.EnumerateDirectories().Chunk(FILE_THREAD_CHUNK_SIZE))
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            await _synchronizationHelper.InvokeAsync(() =>
            {
                foreach (var directory in chunkDirectory)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    Items.Add(_hcTabDirectoryFactory.Invoke(directory));
                }
            });
            await Task.Delay(1); // Won't work without it  ¯\_(ツ)_/¯
        }

        foreach (var fileChunk in hcDirectory.EnumerateFiles().Chunk(FILE_THREAD_CHUNK_SIZE))
        {
            if (ct.IsCancellationRequested)
            {
                return;
            }

            await _synchronizationHelper.InvokeAsync(() =>
            {
                foreach (var file in fileChunk)
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    Items.Add(_hcTabFileFactory.Invoke(file));
                }
            });
            await Task.Delay(1); // Won't work without it ¯\_(ツ)_/¯
        }
    }
}
