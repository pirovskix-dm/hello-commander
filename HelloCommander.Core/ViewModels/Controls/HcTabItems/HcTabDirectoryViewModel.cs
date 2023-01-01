using HelloCommander.Core.Dependencies;
using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.ViewModels.Controls.HcTabItems;

public class HcTabDirectoryViewModel : TabItemViewModelBase, IInitializableComponent<IHcDirectory>
{
    private readonly IHcFileService _hcFileService;
    private readonly IUserInteractionHelper _userInteractionHelper;
    public override bool IsDirectory => true;

    public HcTabDirectoryViewModel(IHcFileService hcFileService, IUserInteractionHelper userInteractionHelper)
    {
        _hcFileService = hcFileService;
        _userInteractionHelper = userInteractionHelper;
    }

    public void Initialize(IHcDirectory hcDirectory)
    {
        Init(hcDirectory);
        Extension = null;
    }

    public override async Task RenameAsync(string name)
    {
        var directory = _hcFileService.GetDirectory(FullName);
        directory.Rename(name);
        Name = directory.Name;
        FullName = directory.Path;
    }

    public override Task Delete()
    {
        var directory = _hcFileService.GetDirectory(FullName);
        directory.Delete();
        return Task.CompletedTask;
    }
}
