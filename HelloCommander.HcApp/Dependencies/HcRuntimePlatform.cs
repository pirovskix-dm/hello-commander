using Avalonia.Platform;
using HelloCommander.Core.Dependencies;

namespace HelloCommander.HcApp.Dependencies;

public class HcRuntimePlatform : IHcRuntimePlatform
{
    private readonly IRuntimePlatform _runtimePlatform;

    public HcRuntimePlatform()
    {
        _runtimePlatform = AvaloniaLocator.Current.GetService<IRuntimePlatform>();
    }

    public HcRuntimePlatformInfo GetRuntimeInfo()
    {
        var runtimeInfo = _runtimePlatform.GetRuntimeInfo();
        return new HcRuntimePlatformInfo
        {
            OperatingSystem = GetHcOperatingSystemType(runtimeInfo.OperatingSystem),
            IsDesktop = runtimeInfo.IsDesktop,
            IsMobile = runtimeInfo.IsMobile,
            IsCoreClr = runtimeInfo.IsCoreClr,
            IsMono = runtimeInfo.IsMono,
            IsDotNetFramework = runtimeInfo.IsDotNetFramework,
            IsUnix = runtimeInfo.IsUnix
        };
    }

    private HcOperatingSystemType GetHcOperatingSystemType(OperatingSystemType operatingSystemType) => operatingSystemType switch
    {
        OperatingSystemType.WinNT => HcOperatingSystemType.WinNT,
        OperatingSystemType.Linux => HcOperatingSystemType.Linux,
        OperatingSystemType.OSX => HcOperatingSystemType.OSX,
        OperatingSystemType.Android => HcOperatingSystemType.Android,
        OperatingSystemType.iOS => HcOperatingSystemType.iOS,
        _ => HcOperatingSystemType.Unknown
    };
}
