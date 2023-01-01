using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Shared.PlatformSupport;

namespace HelloCommander.Themes.Default.Converters;

public class HcTabItemSvgConverter : IValueConverter
{
    private const string ICONS_ASSETS_URI = "avares://HelloCommander.Themes.Default/Assets/IconsDefault";
    private const string FOLDER_ICON_URI = "avares://HelloCommander.Themes.Default/Assets/IconsDefault/folder.svg";
    private const string DEFAULT_ICON_URI = "avares://HelloCommander.Themes.Default/Assets/IconsDefault/file_icon.svg";

    private readonly HashSet<string> _assets;

    public HcTabItemSvgConverter()
    {
        _assets = new AssetLoader()
            .GetAssets(new Uri(ICONS_ASSETS_URI), new Uri(ICONS_ASSETS_URI))
            .Select(a => a.AbsoluteUri)
            .ToHashSet();
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return FOLDER_ICON_URI;
        }

        if (value is not string ext)
        {
            throw new NotSupportedException();
        }

        if (string.IsNullOrWhiteSpace(ext))
        {
            return FOLDER_ICON_URI;
        }

        if (_assets.TryGetValue($"{ICONS_ASSETS_URI}/{ext.TrimStart('.')}.svg", out var uri))
        {
            return uri;
        }

        return DEFAULT_ICON_URI;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return null;
    }
}
