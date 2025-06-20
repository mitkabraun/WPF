using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using WPF_DrawingVisual.Views.Canvas;

namespace WPF_DrawingVisual.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<CanvasItem> _items = [];

    public MainWindowViewModel()
    {
        var canvasItem = new CanvasItem
        {
            X = 10,
            Y = 10,
            Width = 100,
            Height = 100
        };
        canvasItem.Draw();
        Items.Add(canvasItem);

        Start();
    }

    private void Start()
    {
        var random = new Random();
        while (true)
        {
            var canvasItem = new CanvasItem
            {
                X = random.Next(0, 200),
                Y = random.Next(0, 200),
                Width = random.Next(50, 200),
                Height = random.Next(50, 200),
            };
            canvasItem.Draw();
            Items.Add(canvasItem);

            //Thread.Sleep(200);
        }
    }
}
