namespace HelloCommander.Core.Dependencies;

public enum HcYesNoMessageBoxResult
{
    Yes,
    No
}

public record EditTextWindowResult
{
    public bool IsConfirmed { get; set; }
    public string Text { get; set; }
}

public interface IUserInteractionHelper
{
    Task ShowErrorMessage(string text);
    Task ShowSystemErrorMessage(string text);
    Task<HcYesNoMessageBoxResult> ShowYesNoMessageAsync(string text);
    Task<EditTextWindowResult> EditTextWindowAsync(string title, string originalText);
}
