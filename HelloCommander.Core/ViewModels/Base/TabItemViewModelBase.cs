using HelloCommander.Core.Dependencies;

namespace HelloCommander.Core.ViewModels.Base;

public abstract class TabItemViewModelBase : ViewModelBase
{
    public string Name { get; set; }

    public string FullName { get; set; }

    public string Path { get; set; }

    public string Extension { get; set; }

    public bool ActionsEnabled { get; set; }

    public abstract bool IsDirectory { get; }

    protected void Init(IHcPath hcPath)
    {
        Name = hcPath.Name;
        FullName = hcPath.Path;
        Path = hcPath.Path;
        ActionsEnabled = hcPath.ActionsEnabled;
    }

    public abstract Task RenameAsync(string name);

    public abstract Task Delete();
}
