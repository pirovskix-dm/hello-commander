using HelloCommander.Core.Dependencies;
using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.ViewModels.Controls.HcTabItems;

public class HcTabFileViewModel : TabItemViewModelBase, IInitializableComponent<IHcFile>
{
    private const string UNKNOW_EXTENSION = "unknown";

    public override bool IsDirectory => false;

    private readonly IHcFileService _hcFileService;
    private readonly IUserInteractionHelper _userInteractionHelper;

    public HcTabFileViewModel(IHcFileService hcFileService, IUserInteractionHelper userInteractionHelper)
    {
        _hcFileService = hcFileService;
        _userInteractionHelper = userInteractionHelper;
    }

    public void Initialize(IHcFile hcFile)
    {
        Init(hcFile);
        Extension = string.IsNullOrWhiteSpace(hcFile.Extension) ? UNKNOW_EXTENSION : hcFile.Extension;
    }

    public override async Task RenameAsync(string name)
    {
        var file = _hcFileService.GetFile(FullName);
        file.Rename(name);
        Name = file.Name;
        FullName = file.Path;
        Extension = file.Extension;
    }

    public override Task Delete()
    {
        var file = _hcFileService.GetFile(FullName);
        file.Delete();
        return Task.CompletedTask;
    }
}
