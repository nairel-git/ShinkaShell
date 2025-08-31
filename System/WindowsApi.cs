using ShinkaShell;
using System;
using System.Runtime.InteropServices;


public static class WindowsApi
{   

    #region Constants

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_NOACTIVATE = 0x08000000;

    #endregion

    #region PInvokeDeclarations

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    #endregion

    #region CustomFuncions

    public static void SetGhostWindowStyle(IntPtr hwnd)
    {
        int extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_NOACTIVATE);
    }

    public static void BringToForeground(IntPtr hwnd)
    {
        SetForegroundWindow(hwnd);
    }

    #endregion

}