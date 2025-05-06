using System.Text;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace ShinkaShell;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();
        InitializeTrayIcon();

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


    
    private bool isDragging = false;
    private Point lastPosition;


    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool SetActiveWindow(IntPtr hWnd);

    private void Character_MouseDown(object sender, MouseButtonEventArgs e)
    {

        var hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
        SetForegroundWindow(hwnd); // Bring the window to the front
        SetActiveWindow(hwnd); // Set the window as active

        // Log debug information to DebugTextBox
        DebugTextBox.Text += "\nDebug: Moved";
        DebugTextBox.ScrollToEnd(); // Ensure the latest log is visible


        if (e.LeftButton == MouseButtonState.Pressed)
        {
            isDragging = true;
            CharacterImage.CaptureMouse();
            lastPosition = e.GetPosition(this);
        }
    }

    private void Character_MouseMove(object sender, MouseEventArgs e)
    {
        if (isDragging)
        {
            // Log debug information to DebugTextBox
            DebugTextBox.Text += "\nDebug: Moved Drag";
            DebugTextBox.ScrollToEnd(); // Ensure the latest log is visible

            Point currentPosition = e.GetPosition(this);
            Vector delta = currentPosition - lastPosition;

            // Update the Margin of the Image to move it
            Thickness currentMargin = CharacterImage.Margin;

            CharacterImage.Margin = new Thickness(
                currentMargin.Left + delta.X,
                currentMargin.Top + delta.Y,
                currentMargin.Right - delta.X,
                currentMargin.Bottom - delta.Y
            );

            lastPosition = currentPosition;
        }
}

    private void Character_MouseUp(object sender, MouseButtonEventArgs e)
    {
        if (isDragging)
        {
            isDragging = false;
            CharacterImage.ReleaseMouseCapture();
        }
    }

    private System.Windows.Forms.NotifyIcon _notifyIcon;

    private void InitializeTrayIcon()
    {

        Stream stream = Application.GetResourceStream(new Uri("pack://application:,,,/icons/computer.ico")).Stream;
        
        System.Drawing.Icon myIcon = new System.Drawing.Icon(stream);

        stream.Close();
        

        _notifyIcon = new System.Windows.Forms.NotifyIcon
        {
            Icon = myIcon,
            Visible = true,
            Text = "Shinka Shell"
        };

        // Create a context menu for the tray icon
        var contextMenu = new System.Windows.Forms.ContextMenuStrip();
        var settingsItem = new System.Windows.Forms.ToolStripMenuItem("Settings", null, OnSettingsClicked);
        var exitItem = new System.Windows.Forms.ToolStripMenuItem("Exit", null, OnExitClicked);

        contextMenu.Items.Add(settingsItem);
        contextMenu.Items.Add(exitItem);

        _notifyIcon.ContextMenuStrip = contextMenu;

        // Handle double-click to restore the window
        _notifyIcon.DoubleClick += (s, e) => ShowWindow();
    }

    private void OnSettingsClicked(object sender, EventArgs e)
    {
        // Handle settings click
        MessageBox.Show("Settings clicked!", "Shinka Shell");
    }

    private void OnExitClicked(object sender, EventArgs e)
    {
        _notifyIcon.Dispose();
        Application.Current.Shutdown();
    }

    private void ShowWindow()
    {
        this.Show();
        this.WindowState = WindowState.Normal;
        this.Activate();
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        base.OnClosing(e);
        _notifyIcon.Dispose(); // Clean up the tray icon
    }

}