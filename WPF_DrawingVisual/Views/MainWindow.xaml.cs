using WPF_DrawingVisual.ViewModels;

namespace WPF_DrawingVisual.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }
}
