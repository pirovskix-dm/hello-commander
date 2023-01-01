using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace HelloCommander.HcApp.Windows;

public class MainWindow : Window
{
    private Grid _topGrip;
    private Grid _bottomGrip;
    private Grid _leftGrip;
    private Grid _rightGrip;

    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    private void InitializeComponent()
    {
        Cursor = Cursor.Default;
        AvaloniaXamlLoader.Load(this);

        _topGrip = this.FindControl<Grid>("TopGrip");
        _bottomGrip = this.FindControl<Grid>("BottomGrip");
        _leftGrip = this.FindControl<Grid>("LeftGrip");
        _rightGrip = this.FindControl<Grid>("RightGrip");
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        var edge = GetEdge();
        if (edge.HasValue)
        {
            BeginResizeDrag(edge.Value, e);
        }
    }

    private WindowEdge? GetEdge()
    {
        return
            _leftGrip.IsPointerOver ? WindowEdge.West :
            _rightGrip.IsPointerOver ? WindowEdge.East :
            _topGrip.IsPointerOver ? WindowEdge.North :
            _bottomGrip.IsPointerOver ? WindowEdge.South :
            null;
    }
}
