using Avalonia.Input;
using HelloCommander.HcApp.Utils;
using HelloCommander.HcApp.Windows;

namespace HelloCommander.HcApp.Controls;

public class HcTitleBarControl : UserControl
{
    private MainWindow _window;

    public HcTitleBarControl()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        _window = this.GetParentWindow<MainWindow>();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        _window.BeginMoveDrag(e);
    }
}
