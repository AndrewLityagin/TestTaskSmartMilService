using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sms.Test.DesktopApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(ViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public void HideClick(object sender, RoutedEventArgs e)
    {
        this.Hide();
    }
    
    public void CloseClick(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }
}