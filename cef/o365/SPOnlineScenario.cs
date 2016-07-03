using CefSharp;
using CefSharp.OffScreen;
using CefSharp.Internals;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace o365
{
    internal class SPOnlineScenario
    {
        private string _url;
        private string _username;
        private string _password;

        private ChromiumWebBrowser _browser;
        private ManualResetEvent _scenarioFinishedEvent;

        public SPOnlineScenario(string goTo, string username, string password)
        {
            _url = goTo;
            _username = username;
            _password = password;

            _scenarioFinishedEvent = new ManualResetEvent(false);

            _browser = new ChromiumWebBrowser(_url);
        }

        public void WaitForEndOfScenario()
        {
            _scenarioFinishedEvent.WaitOne();
            _scenarioFinishedEvent.Reset();
        }

        public async void Run()
        {
            await LoadPageAsync();

            var host = _browser.Address;
            if (host.StartsWith("https://login.microsoftonline.com/login.srf"))
                await HandleAuthentication();


            //await LoadPageAsync($"{_url}/Shared Documents/Forms/AllItems.aspx");

            var dialogHandler = new DialogHandler();
            _browser.DialogHandler = dialogHandler;

            await UploadDocument();

            Console.ReadKey();
            _scenarioFinishedEvent.Set();
        }

        private Task LoadPageAsync(string address = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            EventHandler<LoadingStateChangedEventArgs> handler = null;
            handler = (sender, args) =>
            {
                //Wait for while page to finish loading not just the first frame
                if (!args.IsLoading)
                {
                    _browser.LoadingStateChanged -= handler;
                    tcs.TrySetResultAsync(true);
                }
            };

            _browser.LoadingStateChanged += handler;

            if (!string.IsNullOrEmpty(address))
            {
                _browser.Load(address);
            }
            return tcs.Task;
        }

        private Task HandleAuthentication()
        {
            var tcs = new TaskCompletionSource<bool>();

            var scriptTask = _browser.EvaluateScriptAsync($"$('#cred_userid_inputtext').val('{_username}'); $('#cred_password_inputtext').val('{_password}'); $('#credentials').submit();");

            scriptTask.ContinueWith(t =>
            {
                Thread.Sleep(45000);

                // Wait for the screenshot to be taken.
                var task = _browser.ScreenshotAsync();
                task.ContinueWith(x =>
                {
                    // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
                    var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

                    Console.WriteLine();
                    Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

                    // Save the Bitmap to the path.
                    // The image type is auto-detected via the ".png" extension.
                    task.Result.Save(screenshotPath);

                    // We no longer need the Bitmap.
                    // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
                    task.Result.Dispose();

                    Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

                    // Tell Windows to launch the saved image.
                    Process.Start(screenshotPath);

                    tcs.TrySetResultAsync(true);
                });
            });
            return tcs.Task;
        }

        private Task UploadDocument()
        {
            var tcs = new TaskCompletionSource<bool>();
            var scriptTask = _browser.EvaluateScriptAsync("var upload_div = document.getElementsByClassName('CommandBarItem-link')[3]; upload_div.click();");


            scriptTask.ContinueWith(t =>
            {
                Console.WriteLine("Get Upload Menu = " + t.Result.Message);
                Thread.Sleep(2000);
                return _browser.EvaluateScriptAsync("var upload_button = document.getElementsByClassName('ContextualMenu-uploadInput')[0]; upload_button.click();");

            }).ContinueWith(t =>
            {
                Thread.Sleep(200);

                // Wait for the screenshot to be taken.
                var task = _browser.ScreenshotAsync();
                task.ContinueWith(x =>
                {
                    // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
                    var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot2.png");

                    Console.WriteLine();
                    Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

                    // Save the Bitmap to the path.
                    // The image type is auto-detected via the ".png" extension.
                    task.Result.Save(screenshotPath);

                    // We no longer need the Bitmap.
                    // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
                    task.Result.Dispose();

                    Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

                    // Tell Windows to launch the saved image.
                    Process.Start(screenshotPath);
                    tcs.TrySetResult(true);
                });
            });
            return tcs.Task;
        }
    }
}
