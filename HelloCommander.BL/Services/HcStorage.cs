using System.Text.Json;
using HelloCommander.Core.Constants;

namespace HelloCommander.BL.Services;

public class HcStorage<T>
{
    private readonly string _path;

    public HcStorage(string file, T defaultData)
    {
        _path = InitFile(file, defaultData);
    }

    public async Task<T> GetAsync()
    {
        var json = await File.ReadAllTextAsync(_path);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }

    public Task SetAsync(T data)
    {
        var json = JsonSerializer.Serialize(data);
        return File.WriteAllTextAsync(_path, json);
    }

    public T Get()
    {
        var json = File.ReadAllText(_path);
        if (string.IsNullOrWhiteSpace(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json);
    }

    public void Set(T data)
    {
        var json = JsonSerializer.Serialize(data);
        File.WriteAllText(_path, json);
    }

    private static string InitFile(string file, T defaultData)
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var baseDirectory = Path.Combine(localAppData, AppConstants.APP_NAME);
        Directory.CreateDirectory(baseDirectory);

        var path = Path.Combine(baseDirectory, file);
        if (!File.Exists(path))
        {
            var json = JsonSerializer.Serialize(defaultData);
            using var sw = File.CreateText(path);
            sw.WriteLine(json);
        }

        return path;
    }
}
