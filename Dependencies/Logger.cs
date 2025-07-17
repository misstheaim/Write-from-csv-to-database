using System.Diagnostics;

namespace Stream_Processing;

public class Logger : IDisposable
{
    private string logFilePath = "log.txt";

    public Logger()
    {
        TextWriterTraceListener tw = new TextWriterTraceListener(File.CreateText(logFilePath));
        Trace.Listeners.Add(tw);
        Trace.WriteLine("Logging starting");
    }

    public void Log(string message)
    {
        Trace.Indent();
        Trace.WriteLine(message);
        Trace.Unindent();
    }

    public void ErrorLog(string message)
    {
        Trace.TraceError(message);
    }
    public void Dispose()
    {
        Trace.WriteLine("Logging ending.");
        Trace.Flush();
    }
}
