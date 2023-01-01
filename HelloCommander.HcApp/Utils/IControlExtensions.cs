namespace HelloCommander.HcApp.Utils;

public static class IControlExtensions
{
    public static T GetFirstParentControl<T>(this IControl control) where T : class, IControl
    {
        var item = control;

        while (item != null)
        {
            if (item is T window)
            {
                return window;
            }

            item = item.Parent;
        }

        return null;
    }

    public static T GetParentWindow<T>(this IControl control) where T : Window
    {
        return control.GetFirstParentControl<T>();
    }
}
