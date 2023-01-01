namespace HelloCommander.Core.Dependencies;

public readonly record struct HcSettings(string HomeDirectory);

public interface ISettingsService
{
    public HcSettings GetSettings();
    Task UpdateHomeDirectory(string path);
}
