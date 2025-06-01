using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace WPF_DrawingVisualFromThread.Views.Canvas;

internal class CustomCanvas : System.Windows.Controls.Canvas
{
    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(CustomCanvas),
        new PropertyMetadata(OnItemsSourceChanged));
    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    private readonly List<Visual> _visuals = [];

    protected override int VisualChildrenCount => _visuals.Count;

    protected override Visual GetVisualChild(int index) => _visuals[index];

    private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is not IEnumerable itemsSource) return;
        if (d is not CustomCanvas customCanvas) return;

        customCanvas.ItemsSourcePropertyChanged(itemsSource);
    }

    private void ItemsSourcePropertyChanged(IEnumerable itemsSource)
    {
        foreach (Visual visual in itemsSource)
        {
            _visuals.Add(visual);
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }
    }
}
