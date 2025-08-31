using System;
using System.Threading;
using System.Windows;

namespace ShinkaShell
{
    public class Program
    {
        private static Mutex SingleInstanceMutex;

        [STAThread]
        public static void Main()
        {
            // single-instance check
            SingleInstanceMutex = new Mutex(true, "ShinkaShell", out bool isSingleInstance);


            if (!isSingleInstance)
            {
                MessageBox.Show("Another instance of ShinkaShell is already running.", "ShinkaShell", MessageBoxButton.OK, MessageBoxImage.Information);
                SingleInstanceMutex.ReleaseMutex();
                return;
            }

            // Create and run the application and main window
            var App = new Application();
            App.Run(new MainWindow());
            
        }
    }
}
