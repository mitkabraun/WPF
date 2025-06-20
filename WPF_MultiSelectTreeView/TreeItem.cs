using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace WPF_MultiSelectTreeView;

public partial class TreeItem : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private bool _isChoosed;
    [ObservableProperty] private bool _isExpanded;
    [ObservableProperty] private bool _isSelected;
    [ObservableProperty] private ObservableCollection<TreeItem> _children = [];
}
