using WPF_DrawingVisualFromThread.ViewModels;

namespace WPF_DrawingVisualFromThread.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }
}
