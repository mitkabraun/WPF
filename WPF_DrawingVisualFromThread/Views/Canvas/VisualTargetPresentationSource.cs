using System;
using System.Windows;
using System.Windows.Media;

namespace WPF_DrawingVisualFromThread.Views.Canvas;

public class VisualTargetPresentationSource : PresentationSource, IDisposable
{
    private readonly VisualTarget _visualTarget;
    private bool _isDisposed;

    public VisualTargetPresentationSource(HostVisual hostVisual)
    {
        _visualTarget = new VisualTarget(hostVisual);
        AddSource();
    }

    public override Visual RootVisual
    {
        get => _visualTarget.RootVisual;
        set
        {
            var oldRoot = _visualTarget.RootVisual;
            _visualTarget.RootVisual = value;
            RootChanged(oldRoot, value);

            if (value is UIElement rootElement)
            {
                rootElement.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                rootElement.Arrange(new Rect(rootElement.DesiredSize));
            }
        }
    }

    protected override CompositionTarget GetCompositionTargetCore() => _visualTarget;

    public override bool IsDisposed => _isDisposed;

    public void Dispose()
    {
        RemoveSource();
        _isDisposed = true;
    }
}
