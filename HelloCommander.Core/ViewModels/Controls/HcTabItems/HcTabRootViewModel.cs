using HelloCommander.Core.Utils;
using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.ViewModels.Controls.HcTabItems;

public class HcTabRootViewModel : TabItemViewModelBase
{
    public override bool IsDirectory => false;

    public HcTabRootViewModel()
    {
        Name = LanguageResources.RootFolderName;
    }

    public override Task RenameAsync(string name)
    {
        return Task.CompletedTask;
    }

    public override Task Delete()
    {
        return Task.CompletedTask;
    }
}
