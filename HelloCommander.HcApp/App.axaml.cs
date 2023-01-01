using Avalonia.Controls.ApplicationLifetimes;
using HelloCommander.Core.ViewModels.Windows;
using HelloCommander.HcApp.Windows;
using Splat;

namespace HelloCommander.HcApp;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Locator.Current.GetService<MainWindowViewModel>();
            desktop.MainWindow = new MainWindow()
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
