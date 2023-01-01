using HelloCommander.Core.Exceptions;
using HelloCommander.Core.Utils;
using SysPath = System.IO.Path;

namespace HelloCommander.BL.Utils;

internal class HcDirectory : HcPath, IHcDirectory
{
    public override string Name => _directoryInfo.Name;

    public override string Path => _directoryInfo.FullName;

    public IHcDirectory Parent { get; private set; }

    public override bool ActionsEnabled => Parent != null;

    private DirectoryInfo _directoryInfo;

    internal HcDirectory(string path) : this(new DirectoryInfo(path))
    {
    }

    internal HcDirectory(DirectoryInfo directoryInfo)
    {
        SetDirectoryInfo(directoryInfo);
    }

    public IEnumerable<IHcDirectory> EnumerateDirectories()
    {
        try
        {
            return _directoryInfo.EnumerateDirectories().Select(d => new HcDirectory(d));
        }
        catch (UnauthorizedAccessException)
        {
            throw new HcForbiddenException(LanguageResources.OpenDirectoryForbidden);
        }
    }

    public IEnumerable<IHcFile> EnumerateFiles()
    {
        try
        {
            return _directoryInfo.EnumerateFiles().Select(f => new HcFile(f));
        }
        catch (UnauthorizedAccessException)
        {
            throw new HcForbiddenException(LanguageResources.OpenDirectoryForbidden);
        }
    }

    public bool DirectoryExists(string directoryName)
    {
        var tempPath = this.Combine(directoryName);
        return Directory.Exists(tempPath);
    }

    public IHcDirectory CreateDirectory(string name)
    {
        var tempPath = this.Combine(name);
        Directory.CreateDirectory(this.Combine(name));
        return new HcDirectory(tempPath);
    }

    public string Combine(IHcPath path)
    {
        return this.Combine(path.Name);
    }

    public string Combine(string path)
    {
        return SysPath.Combine(this.Path, path);
    }

    public bool FileExists(IHcFile file)
    {
        var tempPath = this.Combine(file);
        return File.Exists(tempPath);
    }

    public bool FileExists(string fileName)
    {
        var tempPath = this.Combine(fileName);
        return File.Exists(tempPath);
    }

    public bool DirectoryExists(IHcDirectory directory)
    {
        var tempPath = this.Combine(directory);
        return Directory.Exists(tempPath);
    }

    public IHcDirectory MoveTo(IHcDirectory directory)
    {
        var tempPath = directory.Combine(this);
        _directoryInfo.MoveTo(tempPath);
        return new HcDirectory(tempPath);
    }

    public IHcDirectory MoveTo(IHcDirectory directory, string name)
    {
        var tempPath = directory.Combine(name);
        _directoryInfo.MoveTo(tempPath);
        return new HcDirectory(tempPath);
    }

    public void Rename(string name)
    {
        if (this.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase))
        {
            return;
        }

        if (this.Parent == null)
        {
            return;
        }

        if (this.Parent.DirectoryExists(name))
        {
            throw new HcDirectoryExistsException(name);
        }

        var tempPath = this.Combine(name);
        _directoryInfo.MoveTo(tempPath);

        SetDirectoryInfo(new DirectoryInfo(tempPath));
    }

    public void Delete()
    {
        _directoryInfo.Delete(true);
    }

    private void SetDirectoryInfo(DirectoryInfo directoryInfo)
    {
        _directoryInfo = directoryInfo;
        if (!_directoryInfo.Exists)
        {
            throw new HcDirectoryNotFoundException(_directoryInfo.Name);
        }

        if (_directoryInfo.Parent != null)
        {
            Parent = new HcDirectory(_directoryInfo.Parent);
        }
    }
}
