using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.Dependencies;

public readonly record struct HcBookmark(Guid Id, string Name, string Path, string Extension, int Order);

public interface IBookmarksService
{
    event Func<Task> Changed;

    Task RemoveBookmarkAsync(Guid id);
    Task UpdateName(Guid id, string newName);
    Task<HcBookmark> AddBookmarkAsync(TabItemViewModelBase tabItem);
    Task<List<HcBookmark>> GetBookmarksAsync();
}
