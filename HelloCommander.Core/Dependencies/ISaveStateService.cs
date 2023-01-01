namespace HelloCommander.Core.Dependencies;

public readonly record struct HcOpenedTab(Guid Id, string Path, int Order);

public readonly record struct HcSaveState(ICollection<HcOpenedTab> Tabs);

public interface ISaveStateService
{
    Task<HcSaveState> GetSaveStateAsync();
    Task<Guid> AddTabAsync(string path);
    Task UpdateTabAsync(Guid id, string path);
    Task RemoveTabAsync(Guid id);
}
