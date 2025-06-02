using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

using WPF_DrawingVisualFromThread.Views.Canvas;

namespace WPF_DrawingVisualFromThread.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly Random _random = new();

    [ObservableProperty] private ObservableCollection<object> _items = [];

    public MainWindowViewModel()
    {
        var hostVisual = new HostVisual();
        Items.Add(hostVisual);

        var thread = new Thread(() => Draw(hostVisual)) { IsBackground = true };
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
    }

    private void Draw(HostVisual hostVisual)
    {
        Dispatcher.CurrentDispatcher.InvokeAsync(async () =>
        {
            using var visualTargetSource = new VisualTargetPresentationSource(hostVisual);
            while (true)
            {
                var canvasItems = Step();
                var visual = Print(canvasItems);
                visualTargetSource.RootVisual = visual;
                await Task.Delay(25);
            }
        });
        Dispatcher.Run();
    }

    private List<CanvasItem> Step()
    {
        const int columns = 157;
        const int rows = 83;

        var canvasItems = new List<CanvasItem>();
        for (var i = 0; i < columns; i++)
            for (var j = 0; j < rows; j++)
                canvasItems.Add(new CanvasItem
                {
                    Point = new Point(i * 5, j * 5),
                    Brush = _random.Next(0, 3) switch
                    {
                        0 => Brushes.Green,
                        1 => Brushes.Yellow,
                        _ => Brushes.Red
                    }
                });
        return canvasItems;
    }

    private Visual Print(List<CanvasItem> canvasItems)
    {
        var size = new Size(4, 4);
        var drawingVisual = new DrawingVisual();
        using var drawingContext = drawingVisual.RenderOpen();
        foreach (var canvasItem in canvasItems)
        {
            var rect = new Rect(canvasItem.Point, size);
            drawingContext.DrawRectangle(canvasItem.Brush, null, rect);
        }
        return drawingVisual;
    }
}
