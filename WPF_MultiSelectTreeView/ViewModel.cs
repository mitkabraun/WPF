using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF_MultiSelectTreeView;

public partial class ViewModel : ObservableObject
{
    [ObservableProperty] private ObservableCollection<TreeItem> _itemsSource = [];
    [ObservableProperty] private ObservableCollection<object> _choosedItems = [];

    public ViewModel()
    {
        for (var i = 1; i <= 5; i++)
        {
            var root = new TreeItem { Name = $"MultiSelectTreeViewItem {i}", IsExpanded = true };
            for (var j = 1; j <= 3; j++)
            {
                var child = new TreeItem { Name = $"MultiSelectTreeViewItem {i}.{j}" };
                root.Children.Add(child);
                for (var k = 1; k < 10; k++)
                    child.Children.Add(new TreeItem { Name = $"MultiSelectTreeViewItem {i}.{j}.{k}" });
            }
            ItemsSource.Add(root);
        }
    }
}
