using System.Windows;
using System.Windows.Media;

namespace WPF_DrawingVisual.Views.Canvas;

public class CanvasItem : DrawingVisual
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Rect Rect => new(new Point(X, Y), new Size(Width, Height));

    public void Draw()
    {
        using var dc = RenderOpen();
        dc.DrawRectangle(Brushes.Red, null, Rect);
    }
}
