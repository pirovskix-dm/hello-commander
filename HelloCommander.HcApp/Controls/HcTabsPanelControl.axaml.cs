using System.Reactive.Linq;
using HelloCommander.HcApp.Utils;
using HelloCommander.HcApp.Windows;

namespace HelloCommander.HcApp.Controls;

public class HcTabsPanelControl : UserControl
{
    private ListBox _hcTabsPanelListBox;

    public HcTabsPanelControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        _hcTabsPanelListBox = this.FindControl<ListBox>("HcTabsPanelListBox");
    }

    protected override void OnInitialized()
    {
        var mainWindow = this.GetParentWindow<MainWindow>();
        OnResize(mainWindow.ClientSize);

        mainWindow
            .GetObservable(MainWindow.ClientSizeProperty)
            .Skip(1)
            .Subscribe(OnResize);
    }

    private void OnResize(Size size)
    {
        _hcTabsPanelListBox.MaxWidth = size.Width - 180;
    }
}
