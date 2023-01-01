using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.BL.Services;

public class BookmarksService : IBookmarksService
{
    public event Func<Task> Changed;

    private readonly HcStorage<List<HcBookmark>> _bookmarksStorage;

    public BookmarksService(HcStorage<List<HcBookmark>> bookmarksStorage)
    {
        _bookmarksStorage = bookmarksStorage;
    }

    public async Task RemoveBookmarkAsync(Guid id)
    {
        var bookmarks = await _bookmarksStorage.GetAsync();
        var newBookmarks = bookmarks.Where(b => b.Id != id).ToList();
        await _bookmarksStorage.SetAsync(newBookmarks);
        await RaiseChanged();
    }

    public async Task UpdateName(Guid id, string newName)
    {
        var bookmarks = await _bookmarksStorage.GetAsync();
        var ub = bookmarks.First(b => b.Id == id);
        bookmarks.Remove(ub);
        bookmarks.Add(new HcBookmark(ub.Id, newName, ub.Path, ub.Extension, ub.Order));
        await _bookmarksStorage.SetAsync(bookmarks);
        await RaiseChanged();
    }

    public async Task<HcBookmark> AddBookmarkAsync(TabItemViewModelBase tabItem)
    {
        var bookmarks = await _bookmarksStorage.GetAsync();
        var order = bookmarks.Any() ? bookmarks.Max(b => b.Order) + 1 : 1;
        var newBookmark = new HcBookmark(Guid.NewGuid(), tabItem.Name, tabItem.Path, tabItem.Extension, order);
        bookmarks.Add(newBookmark);
        await _bookmarksStorage.SetAsync(bookmarks);
        await RaiseChanged();
        return newBookmark;
    }

    public async Task<List<HcBookmark>> GetBookmarksAsync()
    {
        return (await _bookmarksStorage.GetAsync()).OrderBy(b => b.Order).ToList();
    }

    private Task RaiseChanged()
    {
        return Changed?.Invoke() ?? Task.CompletedTask;
    }
}
