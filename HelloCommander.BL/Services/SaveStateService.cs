namespace HelloCommander.BL.Services;

public class SaveStateService : ISaveStateService
{
    private readonly HcStorage<HcSaveState> _saveStateStorage;

    public SaveStateService(HcStorage<HcSaveState> saveStateStorage)
    {
        _saveStateStorage = saveStateStorage;
    }

    public async Task<HcSaveState> GetSaveStateAsync()
    {
        return await _saveStateStorage.GetAsync();
    }

    public async Task<Guid> AddTabAsync(string path)
    {
        var id = Guid.NewGuid();
        var state = await _saveStateStorage.GetAsync();
        var order = state.Tabs.Max(t => t.Order) + 1;
        state.Tabs.Add(new HcOpenedTab(id, path, order));
        await _saveStateStorage.SetAsync(state);
        return id;
    }

    public async Task UpdateTabAsync(Guid id, string path)
    {
        var state = await _saveStateStorage.GetAsync();
        var tab = state.Tabs.First(t => t.Id == id);
        state.Tabs.Remove(tab);
        state.Tabs.Add(new HcOpenedTab(id, path, tab.Order));
        await _saveStateStorage.SetAsync(state);
    }

    public async Task RemoveTabAsync(Guid id)
    {
        var state = await _saveStateStorage.GetAsync();
        var tab = state.Tabs.First(t => t.Id == id);
        state.Tabs.Remove(tab);
        await _saveStateStorage.SetAsync(state);
    }
}
