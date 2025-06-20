using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_MultiSelectTreeView;

public class MultiSelectTreeView : TreeView
{
    private readonly HashSet<MultiSelectTreeViewItem> _choosedTreeViewItems = [];

    private MultiSelectTreeViewItem _current;

    public static readonly DependencyProperty ChoosedItemsProperty = DependencyProperty.Register(
        nameof(ChoosedItems),
        typeof(ObservableCollection<object>),
        typeof(MultiSelectTreeView));
    public ObservableCollection<object> ChoosedItems
    {
        get => (ObservableCollection<object>)GetValue(ChoosedItemsProperty);
        set => SetValue(ChoosedItemsProperty, value);
    }

    protected override DependencyObject GetContainerForItemOverride() => new MultiSelectTreeViewItem();

    protected override bool IsItemItsOwnContainerOverride(object item) => item is MultiSelectTreeViewItem;

    protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
    {
        if (Keyboard.IsKeyDown(Key.LeftShift) && _current is not null)
        {
            if (FindParent(this, e.NewValue) is { } multiSelectTreeViewItem)
                if (GetRange(_current, multiSelectTreeViewItem) is { Count: > 0 } choosedItems)
                {
                    foreach (var choosedItem in choosedItems)
                        Add(choosedItem);
                }
            return;
        }

        Clear();
        if (FindParent(this, e.NewValue) is { } multiSelectTreeViewItem2)
            Add(multiSelectTreeViewItem2);
    }

    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
        if (e.OriginalSource is Grid { TemplatedParent: ScrollViewer }) return;
        if (FindParent<ScrollBar>(e.OriginalSource as DependencyObject) is not null) return;

        Focus();

        if (e.ClickCount > 1) return;
        if (FindParent<ToggleButton>(e.OriginalSource as DependencyObject) is not null) return;

        if (FindParent<MultiSelectTreeViewItem>(e.OriginalSource as DependencyObject) is not { } multiSelectTreeViewItem) return;

        e.Handled = true;

        if (Keyboard.IsKeyDown(Key.LeftCtrl))
        {
            Toggle(multiSelectTreeViewItem);
            return;
        }

        Clear();

        if (Keyboard.IsKeyDown(Key.LeftShift) && _current is not null)
        {
            if (GetRange(_current, multiSelectTreeViewItem) is { Count: > 0 } choosedItems)
            {
                foreach (var choosedItem in choosedItems)
                    Add(choosedItem);
                multiSelectTreeViewItem.IsSelected = true;
            }

            return;
        }

        Add(multiSelectTreeViewItem);
        multiSelectTreeViewItem.IsSelected = true;
    }

    protected override void OnPreviewMouseMove(MouseEventArgs e)
    {
        if (e.LeftButton != MouseButtonState.Pressed) return;
        if (Keyboard.IsKeyDown(Key.LeftCtrl)) return;

        if (FindParent<MultiSelectTreeViewItem>(e.OriginalSource as DependencyObject) is { } multiSelectTreeViewItem)
            multiSelectTreeViewItem.IsSelected = true;
    }

    private void Add(MultiSelectTreeViewItem multiSelectTreeViewItem)
    {
        if (multiSelectTreeViewItem.IsChoosed) return;

        multiSelectTreeViewItem.IsChoosed = true;

        _choosedTreeViewItems.Add(multiSelectTreeViewItem);
        ChoosedItems.Add(multiSelectTreeViewItem.DataContext);

        _current = multiSelectTreeViewItem;
    }

    private void Remove(MultiSelectTreeViewItem multiSelectTreeViewItem)
    {
        multiSelectTreeViewItem.IsChoosed = false;

        ChoosedItems.Remove(multiSelectTreeViewItem.DataContext);
        _choosedTreeViewItems.Remove(multiSelectTreeViewItem);
    }

    private void Toggle(MultiSelectTreeViewItem multiSelectTreeViewItem)
    {
        if (multiSelectTreeViewItem.IsChoosed)
            Remove(multiSelectTreeViewItem);
        else
            Add(multiSelectTreeViewItem);
    }

    private void Clear()
    {
        foreach (var multiSelectTreeViewItem in _choosedTreeViewItems)
            multiSelectTreeViewItem.IsChoosed = false;
        _choosedTreeViewItems.Clear();
        ChoosedItems.Clear();
    }

    private static T FindParent<T>(DependencyObject child) where T : DependencyObject
    {
        while (true)
        {
            var parentObject = VisualTreeHelper.GetParent(child);
            switch (parentObject)
            {
                case null:
                    return null;
                case T parent:
                    return parent;
                default:
                    child = parentObject;
                    break;
            }
        }
    }

    private static MultiSelectTreeViewItem FindParent(ItemsControl itemsControl, object obj)
    {
        if (itemsControl == null) return null;
        if (itemsControl.ItemContainerGenerator.ContainerFromItem(obj) is MultiSelectTreeViewItem item) return item;

        foreach (var i in itemsControl.Items)
        {
            var tvi2 = itemsControl.ItemContainerGenerator.ContainerFromItem(i) as MultiSelectTreeViewItem;
            item = FindParent(tvi2, obj);
            if (item != null) return item;
        }
        return null;
    }

    private List<MultiSelectTreeViewItem> GetRange(MultiSelectTreeViewItem start, MultiSelectTreeViewItem end)
    {
        var items = GetItems(this, false);
        var startIndex = items.IndexOf(start);
        var endIndex = items.IndexOf(end);

        if (startIndex > endIndex)
        {
        }

        switch (startIndex)
        {
            case -1 when endIndex == -1:
                return [];
            case -1:
                return [items[endIndex]];
        }

        if (endIndex == -1)
            return [items[startIndex]];

        var rangeStart = Math.Min(startIndex, endIndex);
        var rangeEnd = Math.Max(startIndex, endIndex);

        return items.GetRange(rangeStart, rangeEnd - rangeStart + 1);
    }

    private static List<MultiSelectTreeViewItem> GetItems(ItemsControl parentItem, bool includeCollapsedItems, List<MultiSelectTreeViewItem> itemList = null)
    {
        itemList ??= [];
        for (var index = 0; index < parentItem.Items.Count; index++)
        {
            var item = parentItem.ItemContainerGenerator.ContainerFromIndex(index) as MultiSelectTreeViewItem;
            if (item == null) continue;

            itemList.Add(item);
            if (includeCollapsedItems || item.IsExpanded)
                GetItems(item, includeCollapsedItems, itemList);
        }
        return itemList;
    }
}
