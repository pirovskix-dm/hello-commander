using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.Styling;

namespace HelloCommander.HcApp.Controls;

public class HcContextMenu : ContextMenu, IStyleable
{
    Type IStyleable.StyleKey => typeof(ContextMenu);

    public HcContextMenu()
    {
        HorizontalOffset = -3;
        VerticalOffset = -3;
        PlacementMode = PlacementMode.Pointer;

        InputManager.Instance?.Process.Subscribe(ListenForNonClientClick);
    }

    protected override void OnPointerLeave(PointerEventArgs e)
    {
        Close();
        base.OnPointerLeave(e);
    }

    private void ListenForNonClientClick(RawInputEventArgs e)
    {
        var mouse = e as RawPointerEventArgs;
        if (mouse?.Type == RawPointerEventType.RightButtonDown)
        {
            Close();
        }
    }
}
