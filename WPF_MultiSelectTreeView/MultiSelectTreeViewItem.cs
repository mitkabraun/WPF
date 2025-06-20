using System.Windows;
using System.Windows.Controls;

namespace WPF_MultiSelectTreeView;

public class MultiSelectTreeViewItem : TreeViewItem
{
    public static readonly DependencyProperty IsChoosedProperty = DependencyProperty.Register(
        nameof(IsChoosed),
        typeof(bool),
        typeof(MultiSelectTreeViewItem),
        new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    public bool IsChoosed
    {
        get => (bool)GetValue(IsChoosedProperty);
        set => SetValue(IsChoosedProperty, value);
    }

    protected override DependencyObject GetContainerForItemOverride() => new MultiSelectTreeViewItem();
}
