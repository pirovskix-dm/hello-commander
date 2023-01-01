namespace HelloCommander.HcApp.Windows.Dialogs;

public class MessageBox : Window
{
    public enum MessageBoxButtons
    {
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }

    public enum MessageBoxResult
    {
        Ok,
        Cancel,
        Yes,
        No
    }

    public MessageBox()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    // https://stackoverflow.com/questions/55706291/how-to-show-a-message-box-in-avaloniaui-beta
    public static Task<MessageBoxResult> Show(Window parent, string text, string title, MessageBoxButtons buttons)
    {
        var msgbox = new MessageBox()
        {
            Title = title
        };
        msgbox.FindControl<TextBlock>("Text").Text = text;
        var buttonPanel = msgbox.FindControl<StackPanel>("Buttons");

        var res = MessageBoxResult.Ok;

        void AddButton(string caption, MessageBoxResult r, bool def = false)
        {
            var btn = new Button { Content = caption };
            btn.Click += delegate
            {
                res = r;
                msgbox.Close();
            };
            buttonPanel.Children.Add(btn);
            if (def)
            {
                res = r;
            }
        }

        switch (buttons)
        {
            case MessageBoxButtons.Ok:
            case MessageBoxButtons.OkCancel:
                AddButton("Ok", MessageBoxResult.Ok, true);
                break;
            case MessageBoxButtons.YesNo:
            case MessageBoxButtons.YesNoCancel:
                AddButton("Yes", MessageBoxResult.Yes);
                AddButton("No", MessageBoxResult.No, true);
                break;
        }

        if (buttons is MessageBoxButtons.OkCancel or MessageBoxButtons.YesNoCancel)
        {
            AddButton("Cancel", MessageBoxResult.Cancel, true);
        }

        var tcs = new TaskCompletionSource<MessageBoxResult>();
        msgbox.Closed += delegate { tcs.TrySetResult(res); };
        if (parent != null)
        {
            msgbox.ShowDialog(parent);
        }
        else
        {
            msgbox.Show();
        }

        return tcs.Task;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
