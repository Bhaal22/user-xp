using CefSharp;
using System;
using System.Threading;

namespace o365
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = new CefSettings();
            // Disable GPU in WPF and Offscreen examples until #1634 has been resolved
            settings.CefCommandLineArgs.Add("disable-gpu", "1");

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);


            var url = args[0];
            var username = args[1];
            var password = args[2];

            var scenario = new SPOnlineScenario(url, username, password);

            scenario.Run();

            // We have to wait for something, otherwise the process will exit too soon.
            scenario.WaitForEndOfScenario();

            // Clean up Chromium objects.  You need to call this in your application otherwise
            // you will get a crash when closing.
            Cef.Shutdown();
        }
    }
}
