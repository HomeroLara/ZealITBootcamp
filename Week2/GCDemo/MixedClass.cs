using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace GCDemo;

public class MixedClass : IDisposable
{
    private StreamWriter _writer;
    private Microsoft.Office.Interop.Excel.Application _excel;

    private bool disposedValue;

    public void StartWriting()
    {
        _writer = new StreamWriter("output.txt");
        _excel = new Microsoft.Office.Interop.Excel.Application();
    }

    [SupportedOSPlatform("windows")]
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _writer?.Dispose();
                Console.WriteLine("Disposing of writer");
            }

            // Release COM objects
            if(_excel != null)
            {
                _excel.Quit();
                Marshal.ReleaseComObject(_excel);
                Console.WriteLine("Releasing Excel");
            }

            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    [SupportedOSPlatform("windows")]
    ~MixedClass()
    {
        Dispose(disposing: false);
    }

    [SupportedOSPlatform("windows")]
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}