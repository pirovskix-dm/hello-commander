using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HelloCommander.HcApp.Controls;

public class HcTabItemsControl : UserControl
{
    public HcTabItemsControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
