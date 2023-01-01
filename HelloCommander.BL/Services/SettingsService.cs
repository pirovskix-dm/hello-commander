namespace HelloCommander.BL.Services;

public class SettingsService : ISettingsService
{
    private readonly HcStorage<HcSettings> _settingsStorage;

    private HcSettings _cachedSettings;

    public SettingsService(HcStorage<HcSettings> settingsStorage)
    {
        _settingsStorage = settingsStorage;
        _cachedSettings = _settingsStorage.Get();
    }

    public HcSettings GetSettings()
    {
        return _cachedSettings;
    }

    public async Task UpdateHomeDirectory(string path)
    {
        var newSetting = new HcSettings(path);
        await _settingsStorage.SetAsync(newSetting);
        _cachedSettings = newSetting;
    }
}
