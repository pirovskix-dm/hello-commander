using System.Diagnostics;
using HelloCommander.BL.Utils;
using HelloCommander.Core.Exceptions;

namespace HelloCommander.BL.Services;

public class HcFileService : IHcFileService
{
    public bool HasFileToPaste => _copyFileName != null;

    public event Action CopyStateChanged;

    private static HcPath _copyFileName;

    private readonly HcSystemRoot _hcSystemRoot;

    public HcFileService()
    {
        _hcSystemRoot = new HcSystemRoot();
    }

    public IHcSystemRoot GetSystemRoot()
    {
        return _hcSystemRoot;
    }

    public IHcDirectory GetDirectory(string path)
    {
        return new HcDirectory(path);
    }

    public IHcFile GetFile(string path)
    {
        return new HcFile(path);
    }

    public IHcPath GetPath(string path)
    {
        if (path == _hcSystemRoot.Path)
        {
            return _hcSystemRoot;
        }

        try
        {
            var attr = File.GetAttributes(path);
            if (!attr.HasFlag(FileAttributes.Directory))
            {
                return GetFile(path);
            }

            return GetDirectory(path);
        }
        catch (FileNotFoundException)
        {
            throw new HcDirectoryNotFoundException(path);
        }
    }

    public void Copy(bool isDirectory, string path)
    {
        _copyFileName = isDirectory ? new HcDirectory(path) : new HcFile(path);
        CopyStateChanged?.Invoke();
    }

    public async Task<IHcPath> PastAsync(string destDirName, Func<string, Task<bool>> replace)
    {
        if (!HasFileToPaste)
        {
            return null;
        }

        var destDir = new HcDirectory(destDirName);

        IHcPath copyPath = _copyFileName switch
        {
            HcDirectory sourceDirectory => await DirectoryCopyAsync(sourceDirectory, destDir, replace),
            HcFile sourceFile => await FileCopyAsync(sourceFile, destDir, replace),
            _ => null
        };

        _copyFileName = null;

        return copyPath;
    }

    public void RunFile(IHcFile file)
    {
        Process.Start(new ProcessStartInfo(file.Path)
        {
            UseShellExecute = true
        });
    }

    private string GetNewFileNameForDuplicate(IHcDirectory directory, IHcFile file)
    {
        var newFileName = file.Name;

        var i = 1;
        while (directory.FileExists(newFileName))
        {
            newFileName = $"{i++}_{file.Name}";
        }

        return newFileName;
    }

    private async Task<IHcFile> FileCopyAsync(IHcFile sourceFile, IHcDirectory destDir, Func<string, Task<bool>> replace)
    {
        if (!destDir.FileExists(sourceFile))
        {
            return sourceFile.CopyTo(destDir);
        }

        if (sourceFile.Directory.Path == destDir.Path)
        {
            var newName = GetNewFileNameForDuplicate(destDir, sourceFile);
            return sourceFile.CopyTo(destDir, newName);
        }

        if (!await replace(sourceFile.Name))
        {
            return null;
        }

        sourceFile.CopyTo(destDir, true);
        return null;
    }

    private async Task<IHcDirectory> DirectoryCopyAsync(IHcDirectory sourceDir, IHcDirectory destDir, Func<string, Task<bool>> replace)
    {
        var exists = destDir.DirectoryExists(sourceDir);

        var newDir = destDir.CreateDirectory(sourceDir.Name);

        foreach (var file in sourceDir.EnumerateFiles())
        {
            if (!newDir.FileExists(file))
            {
                file.CopyTo(newDir);
            }
            else if (await replace(file.Name))
            {
                file.CopyTo(newDir, true);
            }
        }

        foreach (var subDir in sourceDir.EnumerateDirectories())
        {
            await DirectoryCopyAsync(subDir, newDir, replace);
        }

        return exists ? null : newDir;
    }
}
