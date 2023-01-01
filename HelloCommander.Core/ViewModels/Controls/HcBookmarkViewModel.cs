using HelloCommander.Core.ViewModels.Base;

namespace HelloCommander.Core.ViewModels.Controls;

public class HcBookmarkViewModel : ViewModelBase
{
    public Guid Id { get; set; }

    public string Path { get; set; }

    public string Header { get; set; }

    public string Extension { get; set; }

    public AsyncDelegateCommand<HcBookmarkViewModel> ClickCommand { get; set; }

    public AsyncDelegateCommand<HcBookmarkViewModel> DeleteCommand { get; set; }

    public AsyncDelegateCommand<HcBookmarkViewModel> RenameCommand { get; set; }
}
