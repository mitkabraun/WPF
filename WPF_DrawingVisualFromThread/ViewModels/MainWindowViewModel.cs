using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    private readonly Stopwatch _fpsStopwatch = Stopwatch.StartNew();

    private int _frameCount;

    [ObservableProperty] private ObservableCollection<HostVisual> _items = [];
    [ObservableProperty] private int _delay = 1;
    [ObservableProperty] private int _fps;

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
                var customCanvasItems = Step();
                var visual = Print(customCanvasItems);
                visualTargetSource.RootVisual = visual;
                UpdateFpsCounterAsync();
                await Task.Delay(Delay);
            }
        });
        Dispatcher.Run();
    }

    private void UpdateFpsCounterAsync()
    {
        _frameCount++;
        if (_fpsStopwatch.Elapsed.TotalSeconds >= 1)
        {
            Fps = _frameCount;
            _frameCount = 0;
            _fpsStopwatch.Restart();
        }
    }

    private List<CustomCanvasItem> Step()
    {
        const int columns = 157;
        const int rows = 83;

        var customCanvasItems = new List<CustomCanvasItem>();
        for (var i = 0; i < columns; i++)
            for (var j = 0; j < rows; j++)
            {
                var point = new Point(i * 5, j * 5);
                var next = _random.Next(0, 3);
                Brush brush = next switch
                {
                    0 => Brushes.Green,
                    1 => Brushes.Yellow,
                    _ => Brushes.Red
                };
                customCanvasItems.Add(new CustomCanvasItem(point, brush));
            }
        return customCanvasItems;
    }

    private Visual Print(List<CustomCanvasItem> customCanvasItems)
    {
        var drawingVisual = new DrawingVisual();
        using var drawingContext = drawingVisual.RenderOpen();
        foreach (var customCanvasItem in customCanvasItems)
        {
            var rect = new Rect(customCanvasItem.Point, new Size(4, 4));
            drawingContext.DrawRectangle(customCanvasItem.Brush, null, rect);
        }
        return drawingVisual;
    }
}
