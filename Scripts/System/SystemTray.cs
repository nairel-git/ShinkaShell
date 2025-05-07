using System.Text;
using System.Windows;
using System;

namespace ShinkaShell;

public partial class MainWindow
{
    private System.Windows.Forms.NotifyIcon _notifyIcon;
    private void InitializeTrayIcon()
    {  
        //Load Embedded Icon from Resources (it has to be a absolute path)
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
        var resetPosition = new System.Windows.Forms.ToolStripMenuItem("Bring him back!!!!", null, OnResetPositionClicked);
        var settingsItem = new System.Windows.Forms.ToolStripMenuItem("Settings", null, OnSettingsClicked);
        var exitItem = new System.Windows.Forms.ToolStripMenuItem("Exit", null, OnExitClicked);


        contextMenu.Items.Add(resetPosition);
        contextMenu.Items.Add(settingsItem);
        contextMenu.Items.Add(exitItem);

        _notifyIcon.ContextMenuStrip = contextMenu;
    }

    private void OnResetPositionClicked(object sender, EventArgs e)
    {
        // Reset the character position
        ResetPosition();
    }

    private void OnSettingsClicked(object sender, EventArgs e)
    {
        // Handle settings click
        MessageBox.Show("Settings", "ShinkaShell", MessageBoxButton.OK, MessageBoxImage.Information);
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
