namespace HelloCommander.Core.Dependencies;

public interface IHcPath
{
    string Name { get; }
    string Path { get; }
    bool ActionsEnabled { get; }
}

public interface IHcFile : IHcPath
{
    IHcDirectory Directory { get; }
    string Extension { get; }

    IHcFile CopyTo(IHcDirectory directory, bool overwrite = false);
    IHcFile CopyTo(IHcDirectory directory, string name, bool overwrite = false);
    void Rename(string name);
    void Delete();
}

public interface IHcDirectory : IHcPath
{
    IHcDirectory Parent { get; }

    IEnumerable<IHcDirectory> EnumerateDirectories();
    IEnumerable<IHcFile> EnumerateFiles();
    IHcDirectory CreateDirectory(string name);
    bool DirectoryExists(string name);
    bool DirectoryExists(IHcDirectory directory);
    string Combine(IHcPath path);
    string Combine(string path);
    bool FileExists(IHcFile file);
    bool FileExists(string fileName);
    IHcDirectory MoveTo(IHcDirectory directory);
    IHcDirectory MoveTo(IHcDirectory directory, string name);
    void Rename(string name);
    void Delete();
}

public interface IHcSystemRoot : IHcPath
{
    IEnumerable<IHcDirectory> GetLogicalDrives();
}

public interface IHcFileService
{
    bool HasFileToPaste { get; }
    event Action CopyStateChanged;

    IHcSystemRoot GetSystemRoot();
    IHcDirectory GetDirectory(string path);
    IHcFile GetFile(string path);
    IHcPath GetPath(string path);
    void Copy(bool isDirectory, string path);
    Task<IHcPath> PastAsync(string destDirName, Func<string, Task<bool>> replace);
    void RunFile(IHcFile file);
}
