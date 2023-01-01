using Avalonia.Controls.ApplicationLifetimes;
using HelloCommander.Core.Dependencies;
using HelloCommander.HcApp.Windows.Dialogs;

namespace HelloCommander.HcApp.Dependencies;

public class UserInteractionHelper : IUserInteractionHelper
{
    public async Task ShowErrorMessage(string text)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await MessageBox.Show(desktop.MainWindow, text, string.Empty, MessageBox.MessageBoxButtons.Ok);
            desktop.MainWindow.Activate(); // TODO: doesn't work;
        }
    }

    public async Task ShowSystemErrorMessage(string text)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await MessageBox.Show(desktop.MainWindow, text, string.Empty, MessageBox.MessageBoxButtons.Ok);
            desktop.MainWindow.Activate(); // TODO: doesn't work;
        }
    }

    public async Task<HcYesNoMessageBoxResult> ShowYesNoMessageAsync(string text)
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var result = await MessageBox.Show(desktop.MainWindow, text, string.Empty, MessageBox.MessageBoxButtons.YesNo);
            desktop.MainWindow.Activate(); // TODO: doesn't work;
            return result switch
            {
                MessageBox.MessageBoxResult.Yes => HcYesNoMessageBoxResult.Yes,
                MessageBox.MessageBoxResult.No => HcYesNoMessageBoxResult.No,
                _ => HcYesNoMessageBoxResult.No
            };
        }

        return HcYesNoMessageBoxResult.No;
    }

    public async Task<EditTextWindowResult> EditTextWindowAsync(string title, string originalText)
    {
        var result = new EditTextWindowResult()
        {
            IsConfirmed = false,
            Text = string.Empty
        };

        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            await HcEditNameWindow.Show(desktop.MainWindow, title, originalText, (isConfirmed, text) =>
            {
                result.IsConfirmed = isConfirmed;
                result.Text = text;
            });
            desktop.MainWindow.Activate(); // TODO: doesn't work;
        }

        return result;
    }
}
