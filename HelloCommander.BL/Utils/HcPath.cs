using SysPath = System.IO.Path;

namespace HelloCommander.BL.Utils;

public abstract class HcPath : IHcPath
{
    public abstract string Name { get; }

    public abstract string Path { get; }

    public abstract bool ActionsEnabled { get; }
}
