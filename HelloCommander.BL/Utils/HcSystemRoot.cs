using HelloCommander.Core.Utils;

namespace HelloCommander.BL.Utils;

public class HcSystemRoot : IHcSystemRoot
{
    public string Name => LanguageResources.RootFolderName;
    public string Path => System.IO.Path.DirectorySeparatorChar.ToString();
    public bool ActionsEnabled => false;

    public IEnumerable<IHcDirectory> GetLogicalDrives()
    {
        return Directory.GetLogicalDrives().Select(d => new HcDirectory(d));
    }
}
