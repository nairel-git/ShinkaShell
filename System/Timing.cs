using System;
using System.Diagnostics;
using System.Threading;

public static class Timing
{   

    private static Stopwatch _DeltaTimeTimer = new Stopwatch();
    private static float _DeltaTime;

    public static void DeltaTimeStart()
    {
        _DeltaTimeTimer.Start();
    }
    
    public static void DeltaTimeRestart()
    {
        _DeltaTime = (float)_DeltaTimeTimer.Elapsed.TotalSeconds;
        _DeltaTimeTimer.Restart();
    }
    public static float DeltaTimeGet()
    {
        return _DeltaTime;
    }


}