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

namespace ShinkaShell
{
    public partial class MainWindow
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private void InitializeTrayIcon()
        {  

            var iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Resources/icons/computer.ico")).Stream;
            
            var myIcon = new System.Drawing.Icon(iconStream);

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

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            _notifyIcon.Dispose();
        }
    }
}