namespace WPF_MultiSelectTreeView;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ViewModel();
    }
}
