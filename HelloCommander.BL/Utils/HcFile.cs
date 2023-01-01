using HelloCommander.Core.Exceptions;

namespace HelloCommander.BL.Utils;

internal class HcFile : HcPath, IHcFile
{
    public override string Name => _fileInfo.Name;

    public override string Path => _fileInfo.FullName;

    public override bool ActionsEnabled => true;

    public string Extension => _fileInfo.Extension;

    public IHcDirectory Directory { get; private set; }

    private FileInfo _fileInfo;

    internal HcFile(string path) : this(new FileInfo(path))
    {
    }

    internal HcFile(FileInfo fileInfo)
    {
        SetFileInfo(fileInfo);
    }

    public IHcFile CopyTo(IHcDirectory directory, bool overwrite = false)
    {
        var tempPath = directory.Combine(this);
        _fileInfo.CopyTo(tempPath, overwrite);
        return new HcFile(tempPath);
    }

    public IHcFile CopyTo(IHcDirectory directory, string name, bool overwrite = false)
    {
        var tempPath = directory.Combine(name);
        _fileInfo.CopyTo(tempPath, overwrite);
        return new HcFile(tempPath);
    }

    public void Rename(string name)
    {
        if (this.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }

        if (this.Directory.FileExists(name))
        {
            throw new HcFileExistsException(name);
        }

        var tempPath = this.Directory.Combine(name);
        _fileInfo.MoveTo(tempPath);

        SetFileInfo(new FileInfo(tempPath));
    }

    public void Delete()
    {
        _fileInfo.Delete();
    }

    private void SetFileInfo(FileInfo fileInfo)
    {
        _fileInfo = fileInfo;
        if (!_fileInfo.Exists)
        {
            throw new HcFileNotFoundException(_fileInfo.Name);
        }

        if (_fileInfo.Directory != null)
        {
            Directory = new HcDirectory(_fileInfo.Directory);
        }
    }
}
