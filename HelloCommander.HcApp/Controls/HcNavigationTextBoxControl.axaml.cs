using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HelloCommander.HcApp.Controls;

public class HcNavigationTextBoxControl : UserControl
{
    public HcNavigationTextBoxControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
