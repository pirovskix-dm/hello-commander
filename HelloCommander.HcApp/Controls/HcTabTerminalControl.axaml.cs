using Avalonia.Input;
using Avalonia.Interactivity;

namespace HelloCommander.HcApp.Controls;

public class HcTabTerminalControl : UserControl
{
    public HcTabTerminalControl()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void InputElement_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;
        }
    }
}
