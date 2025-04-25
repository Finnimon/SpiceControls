using System.Diagnostics;

namespace Spice.Controls.Core;

public interface ILogger
{
    public void Log(Exception ex, string msg = "");

    public static ILogger DebugLogger => new InternalDebugLogger();

    private class InternalDebugLogger : ILogger
    {
        public void Log(Exception ex, string? msg = null)
            => Debug.WriteLine($"{msg?.Append('\n') ?? ""}{ex}");
    }
}