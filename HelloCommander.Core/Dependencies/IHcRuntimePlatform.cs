namespace HelloCommander.Core.Dependencies;

public enum HcOperatingSystemType
{
    Unknown,
    WinNT,
    Linux,
    OSX,
    Android,
    iOS
}

public struct HcRuntimePlatformInfo
{
    public HcOperatingSystemType OperatingSystem { get; set; }
    public bool IsDesktop { get; set; }
    public bool IsMobile { get; set; }
    public bool IsCoreClr { get; set; }
    public bool IsMono { get; set; }
    public bool IsDotNetFramework { get; set; }
    public bool IsUnix { get; set; }
}

public interface IHcRuntimePlatform
{
    HcRuntimePlatformInfo GetRuntimeInfo();
}
