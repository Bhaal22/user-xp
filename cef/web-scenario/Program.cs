using CefSharp;
using System;

namespace webscenario
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = new CefSettings();
            // Disable GPU in WPF and Offscreen examples until #1634 has been resolved
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            settings.CefCommandLineArgs.Add("disable-web-security", "1");
            settings.RemoteDebuggingPort = 8088;

            //var t1 = Cef.AddCrossOriginWhitelistEntry("https://login.microsoftonline.com", "https", string.Empty, true);
            //var t2 = Cef.AddCrossOriginWhitelistEntry("https://portal.office365.com", "https", string.Empty, true);

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, shutdownOnProcessExit: true, performDependencyCheck: true);

            var techno = args[0];
            var url = args[1];
            var username = args[2];
            var password = args[3];


            if (techno == "gmail")
            {
                var scenario = new GmailScenario(url, username, password);
                scenario.Run();

                // We have to wait for something, otherwise the process will exit too soon.
                scenario.WaitForEndOfScenario();
            }
            else if (techno == "sharepoint")
            {
                var scenario = new SPOnlineScenario(url, username, password);
                scenario.Run();

                // We have to wait for something, otherwise the process will exit too soon.
                scenario.WaitForEndOfScenario();
            }
            else
                Console.WriteLine("Unsupported technology");

            // Clean up Chromium objects.  You need to call this in your application otherwise
            // you will get a crash when closing.
            Cef.Shutdown();
        }
    }
}
