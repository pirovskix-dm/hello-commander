using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using HelloCommander.HcApp.Utils;
using HelloCommander.HcApp.Windows;

namespace HelloCommander.HcApp.Controls;

public class HcSystemTitleBarButtonsControl : UserControl
{
    private MainWindow _window;

    public HcSystemTitleBarButtonsControl()
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

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
        {
            lifetime.Shutdown();
        }
    }

    private void ExpandButton_OnClick(object sender, RoutedEventArgs e)
    {
        _window.WindowState = _window.WindowState switch
        {
            WindowState.Normal => WindowState.Maximized,
            WindowState.Maximized => WindowState.Normal,
            _ => _window.WindowState
        };
    }

    private void CollapseButton_OnClick(object sender, RoutedEventArgs e)
    {
        _window.WindowState = WindowState.Minimized;
    }
}
