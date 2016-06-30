using CefSharp;
using System;

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
            Console.ReadKey();

            // Clean up Chromium objects.  You need to call this in your application otherwise
            // you will get a crash when closing.
            Cef.Shutdown();
        }

        public static async void run(IWebBrowser browser)
        {
            


        }

        //private static void BrowserLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        //{
        //    // Check to see if loading is complete - this event is called twice, one when loading starts
        //    // second time when it's finished
        //    // (rather than an iframe within the main frame).
        //    if (!e.IsLoading)
        //    {
        //        // Remove the load event handler, because we only want one snapshot of the initial page.
        //        browser.LoadingStateChanged -= BrowserLoadingStateChanged;

        //        //browser.LoadingStateChanged += BrowserAuthenticationLoadingStateChanged;

        //        scriptTask.ContinueWith(t =>
        //        {
        //            Thread.Sleep(5000);

        //            var scriptTask2 = browser.EvaluateScriptAsync("$('button.o365cs-nav-item.o365cs-nav-button.o365cs-me-nav-item.o365button.ms-bgc-tdr-h.ms-fcl-w').click();");

        //            scriptTask2.ContinueWith(t2 =>
        //            {
        //                //Give the browser a little time to render
        //                Thread.Sleep(5000);

        //                // Wait for the screenshot to be taken.
        //                var task = browser.ScreenshotAsync();
        //                task.ContinueWith(x =>
        //                {
        //                    // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
        //                    var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

        //                    Console.WriteLine();
        //                    Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

        //                    // Save the Bitmap to the path.
        //                    // The image type is auto-detected via the ".png" extension.
        //                    task.Result.Save(screenshotPath);

        //                    // We no longer need the Bitmap.
        //                    // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
        //                    task.Result.Dispose();

        //                    Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

        //                    // Tell Windows to launch the saved image.
        //                    Process.Start(screenshotPath);

        //                    Console.WriteLine("Image viewer launched.  Press any key to exit.");
        //                });
        //            });
        //        });
        //    }
        //}

        //private static void BrowserAuthenticationLoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        //{
        //    // Check to see if loading is complete - this event is called twice, one when loading starts
        //    // second time when it's finished
        //    // (rather than an iframe within the main frame).
        //    if (!e.IsLoading)
        //    {
        //        // Remove the load event handler, because we only want one snapshot of the initial page.
        //         browser.LoadingStateChanged -= BrowserAuthenticationLoadingStateChanged;

        //        var scriptTask = browser.EvaluateScriptAsync("$('button.o365cs-nav-item.o365cs-nav-button.o365cs-me-nav-item.o365button.ms-bgc-tdr-h.ms-fcl-w').click();");

        //        scriptTask.ContinueWith(t =>
        //        {
        //            //Give the browser a little time to render
        //            Thread.Sleep(5000);

        //            // Wait for the screenshot to be taken.
        //            var task = browser.ScreenshotAsync();
        //            task.ContinueWith(x =>
        //            {
        //                // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
        //                var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

        //                Console.WriteLine();
        //                Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

        //                // Save the Bitmap to the path.
        //                // The image type is auto-detected via the ".png" extension.
        //                task.Result.Save(screenshotPath);

        //                // We no longer need the Bitmap.
        //                // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
        //                task.Result.Dispose();

        //                Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

        //                // Tell Windows to launch the saved image.
        //                Process.Start(screenshotPath);

        //                Console.WriteLine("Image viewer launched.  Press any key to exit.");
        //            });
        //        });
        //    }
        //}
    }
}
