using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShinkaShell
{
    public partial class MainWindow : Window
    {
        // These are now created in code and must be accessible to the other partial class files.
        private readonly Canvas MainCanvas;
        internal Image CharacterImage;

        public MainWindow()
        {
            // --- Set Window Properties ---
            this.AllowsTransparency = true;
            this.WindowStyle = WindowStyle.None;
            this.Background = Brushes.Transparent;
            this.Topmost = true;
            this.ShowInTaskbar = false;
            this.ResizeMode = ResizeMode.NoResize;

            // --- Create UI Elements ---
            CharacterImage = new Image
            {
                Width = 200,
                Height = 200,
                Source = new BitmapImage(new Uri("resources/sprites/character.png", UriKind.Relative)),
                Stretch = Stretch.Uniform
            };

            // --- Create Layout and Set Content ---
            MainCanvas = new Canvas();
            MainCanvas.Children.Add(CharacterImage);
            this.Content = MainCanvas;

            InitializeTrayIcon();
            InitCharacter();

            this.Loaded += Window_Loaded;
            CompositionTarget.Rendering += OnRenderFrame;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Timing.DeltaTimeStart();

            // Set the window to be a full-screen overlay
            this.Left = 0;
            this.Top = 0;
            this.Width = SystemParameters.PrimaryScreenWidth;

            //The -1 is to avoid obscuring taskbar making the all windows go over it 
            this.Height = SystemParameters.PrimaryScreenHeight - 1;

            var InteropHelper = new WindowInteropHelper(this);
            WindowsApi.SetGhostWindowStyle(InteropHelper.Handle);
        }

        private void OnRenderFrame(object sender, EventArgs e)
        {
            Timing.DeltaTimeRestart();
            CharacterGravity();
            CharacterUpdate();
        }
    }
}