public class SampleConsoleApp
{
    public static readonly SampleConsoleApp Instance = new SampleConsoleApp();

    public SampleConsoleApp()
    {
        // Simulating initialization delay
        System.Threading.Thread.Sleep(500);
    }
}