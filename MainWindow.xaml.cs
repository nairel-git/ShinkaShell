using System.Text;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShinkaShell;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
        //RenderOptions.SetEdgeMode(this, EdgeMode.Aliased);        
    }
        
    
    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    
    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Set the window's position to (0, 0)
        this.Left = 0;
        this.Top = 0;

        // Set the window's width and height to match the primary screen dimensions
        this.Width = SystemParameters.PrimaryScreenWidth;
        this.Height = SystemParameters.PrimaryScreenHeight - 1;

        var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_NOACTIVATE);
    
    }

    
    private void Shutdown(object sender, RoutedEventArgs e) 
    {
        Application.Current.Shutdown();
    }

}