namespace HelloCommander.HcApp.Windows.Dialogs;

public class HcEditNameWindow : Window
{
    public HcEditNameWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public static Task Show(Window parent, string title, string originalText, Action<bool, string> action)
    {
        var editWindow = new HcEditNameWindow();
        var textBlock = editWindow.FindControl<TextBlock>("HcEditNameTextBlock");
        var textBox = editWindow.FindControl<TextBox>("HcEditNameTextBox");
        var okButton = editWindow.Find<Button>("HcEditNameOkButton");
        var cancelButton = editWindow.Find<Button>("HcEditNameCancelButton");

        textBlock.Text = title;
        textBox.Text = originalText ?? string.Empty;

        var tcs = new TaskCompletionSource();
        okButton.Click += (_, __) =>
        {
            action(true, textBox.Text);
            editWindow.Close();
        };
        cancelButton.Click += (_, __) =>
        {
            action(false, originalText);
            editWindow.Close();
        };

        if (parent != null)
        {
            editWindow.ShowDialog(parent);
        }
        else
        {
            editWindow.Show();
        }

        editWindow.Closed += delegate { tcs.TrySetResult(); };
        return tcs.Task;
    }
}
