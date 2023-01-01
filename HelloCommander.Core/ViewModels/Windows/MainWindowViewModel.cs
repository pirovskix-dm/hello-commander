using System.Collections.ObjectModel;
using HelloCommander.Core.Dependencies;
using HelloCommander.Core.Utils;
using HelloCommander.Core.ViewModels.Base;
using HelloCommander.Core.ViewModels.Controls;

namespace HelloCommander.Core.ViewModels.Windows;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<HcTabViewModel> Tabs { get; set; } = new();
    public ObservableCollection<HcBookmarkViewModel> Bookmarks { get; set; } = new();
    public HcTabViewModel SelectedTab { get; set; }
    public AsyncDelegateCommand AddTabCommand { get; set; }
    public AsyncDelegateCommand<HcTabViewModel> CloseTabCommand { get; set; }
    public AsyncDelegateCommand<HcTabViewModel> DuplicateTabCommand { get; set; }
    public AsyncDelegateCommand<HcTabViewModel> AddBookmarkCommand { get; set; }
    public AsyncDelegateCommand HomeCommand { get; set; }
    public AsyncDelegateCommand InitializeCommand { get; set; }

    private readonly Func<Guid, string, Task<HcTabViewModel>> _hcTabFactory;
    private readonly Func<HcBookmarkViewModel> _hcBookmarkFactory;
    private readonly IBookmarksService _bookmarksService;
    private readonly IUserInteractionHelper _userInteractionHelper;
    private readonly ISettingsService _settingsService;
    private readonly ISaveStateService _saveStateService;
    private readonly IHcCommandFactory _hcCommandFactory;

    public MainWindowViewModel(
        Func<Guid, string, Task<HcTabViewModel>> hcTabFactory,
        Func<HcBookmarkViewModel> hcBookmarkFactory,
        IBookmarksService bookmarksService,
        IUserInteractionHelper userInteractionHelper,
        ISettingsService settingsService,
        ISaveStateService saveStateService,
        IHcCommandFactory hcCommandFactory)
    {
        _bookmarksService = bookmarksService;
        _hcTabFactory = hcTabFactory;
        _hcBookmarkFactory = hcBookmarkFactory;
        _userInteractionHelper = userInteractionHelper;
        _settingsService = settingsService;
        _saveStateService = saveStateService;
        _hcCommandFactory = hcCommandFactory;

        _bookmarksService.Changed += OnBookmarkChangedAsync;

        AddTabCommand = hcCommandFactory.CreateAsyncCommand(LanguageResources.AddTabCommand, OnAddTabAsync);
        CloseTabCommand = hcCommandFactory.CreateAsyncCommand<HcTabViewModel>(LanguageResources.CloseTabCommand, OnCloseTab);
        DuplicateTabCommand = hcCommandFactory.CreateAsyncCommand<HcTabViewModel>(LanguageResources.CloseTabCommand, OnDuplicateTabTab);
        AddBookmarkCommand = hcCommandFactory.CreateAsyncCommand<HcTabViewModel>(LanguageResources.AddBookmarkCommand, OnAddBookmarkAsync);
        HomeCommand = hcCommandFactory.CreateAsyncCommand(LanguageResources.HomeCommand, OnHomeAsync);
        InitializeCommand = hcCommandFactory.CreateAsyncCommand(LanguageResources.InitializeMainWindowCommand, OnInitializeAsync);
    }

    private Task OnBookmarkChangedAsync()
    {
        return UpdateBookmarksAsync();
    }

    private Task OnHomeAsync()
    {
        var settings = _settingsService.GetSettings();
        return SelectedTab.TabItemsContext.LoadPathAsync(settings.HomeDirectory);
    }

    private async Task OnInitializeAsync()
    {
        await ReadStateAsync();
        await UpdateBookmarksAsync();
    }

    private async Task OnAddBookmarkAsync(HcTabViewModel hcTabViewModel)
    {
        await _bookmarksService.AddBookmarkAsync(SelectedTab.TabItemsContext.CurrentDirectory);
    }

    private async Task UpdateBookmarksAsync()
    {
        var bookmarks = await _bookmarksService.GetBookmarksAsync();
        Bookmarks = new ObservableCollection<HcBookmarkViewModel>();
        foreach (var bookmark in bookmarks)
        {
            AddNewBookmark(bookmark);
        }
    }

    private void AddNewBookmark(HcBookmark hcBookmark)
    {
        var bookmark = _hcBookmarkFactory.Invoke();
        bookmark.Id = hcBookmark.Id;
        bookmark.Path = hcBookmark.Path;
        bookmark.Header = hcBookmark.Name;
        bookmark.Extension = hcBookmark.Extension;
        bookmark.ClickCommand = _hcCommandFactory.CreateAsyncCommand<HcBookmarkViewModel>(LanguageResources.BookmarkClickCommand, OnBookmarkClickAsync);
        bookmark.DeleteCommand = _hcCommandFactory.CreateAsyncCommand<HcBookmarkViewModel>(LanguageResources.BookmarkDeleteCommand, OnBookmarkDeleteAsync);
        bookmark.RenameCommand = _hcCommandFactory.CreateAsyncCommand<HcBookmarkViewModel>(LanguageResources.BookmarkRenameCommand, OnBookmarkRenameAsync);
        Bookmarks.Add(bookmark);
    }

    private async Task OnBookmarkRenameAsync(HcBookmarkViewModel hcBookmarkViewModel)
    {
        var result = await _userInteractionHelper.EditTextWindowAsync(LanguageResources.RenameBookmarkTitle, hcBookmarkViewModel.Header);

        if (result.IsConfirmed)
        {
            hcBookmarkViewModel.Header = result.Text;
            await _bookmarksService.UpdateName(hcBookmarkViewModel.Id, result.Text);
        }
    }

    private async Task OnBookmarkDeleteAsync(HcBookmarkViewModel hcBookmarkViewModel)
    {
        await _bookmarksService.RemoveBookmarkAsync(hcBookmarkViewModel.Id);
        Bookmarks.Remove(hcBookmarkViewModel);
    }

    private Task OnBookmarkClickAsync(HcBookmarkViewModel hcBookmarkViewModel)
    {
        return SelectedTab.TabItemsContext.LoadPathAsync(hcBookmarkViewModel.Path);
    }

    private async Task ReadStateAsync()
    {
        var state = await _saveStateService.GetSaveStateAsync();
        foreach (var (id, path, order) in state.Tabs.OrderBy(t => t.Order))
        {
            await CreateTab(id, path);
        }
    }

    private async Task OnAddTabAsync()
    {
        var id = await _saveStateService.AddTabAsync(string.Empty);
        await CreateTab(id, null);
    }

    private async Task OnDuplicateTabTab(HcTabViewModel tabViewModel)
    {
        var id = await _saveStateService.AddTabAsync(tabViewModel.TabItemsContext.CurrentDirectory.Path);
        await CreateTab(id, tabViewModel.TabItemsContext.CurrentDirectory.Path);
    }

    private async Task CreateTab(Guid id, string path)
    {
        var newTab = await _hcTabFactory.Invoke(id, path);
        newTab.Id = id;
        Tabs.Add(newTab);
        SelectedTab = newTab;
    }

    private async Task OnCloseTab(HcTabViewModel tab)
    {
        tab.TabItemsContext.StopLoadingItmes();
        Tabs.Remove(tab);
        SelectedTab = Tabs.LastOrDefault();
        await _saveStateService.RemoveTabAsync(tab.Id);
    }
}
