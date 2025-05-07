using System;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;


namespace ShinkaShell;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeTrayIcon();
        InitCharacter();

        CompositionTarget.Rendering += OnRenderFrame;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Timing.DeltaTimeStart();

        // Set the window's position to (0, 0)
        this.Left = 0;
        this.Top = 0;

        // Set the window's width and height to match the primary screen dimensions
        this.Width = SystemParameters.PrimaryScreenWidth;
        this.Height = SystemParameters.PrimaryScreenHeight - 1;

        var CurrentWindow = new WindowInteropHelper(this);

        WindowsApi.SetGhostWindowStyle(CurrentWindow.Handle);
    }

    private void OnRenderFrame(object sender, EventArgs e)
    {
        Timing.DeltaTimeRestart();
        CharacterGravity();
        CharacterImage.RenderTransform = new TranslateTransform(Position.X + PivotOffset.X, Position.Y + PivotOffset.Y);
    }

}