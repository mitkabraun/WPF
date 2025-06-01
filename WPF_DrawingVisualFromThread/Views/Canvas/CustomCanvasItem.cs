using System.Windows;
using System.Windows.Media;

namespace WPF_DrawingVisualFromThread.Views.Canvas;

public class CustomCanvasItem(Point point, Brush brush)
{
    public Point Point { get; } = point;
    public Brush Brush { get; } = brush;
}
