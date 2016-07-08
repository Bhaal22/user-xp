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
    internal class GmailScenario
    {
        private string _url;
        private string _username;
        private string _password;

        private ChromiumWebBrowser _browser;
        private ManualResetEvent _scenarioFinishedEvent;

        public GmailScenario(string goTo, string username, string password)
        {
            _url = goTo;
            _username = username;
            _password = password;

            _scenarioFinishedEvent = new ManualResetEvent(false);

            _browser = new ChromiumWebBrowser(_url);
            
            _browser.DialogHandler = new DialogHandler();
            _browser.JsDialogHandler = new JSDialogHandler();
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
            if (host.StartsWith("https://accounts.google.com/ServiceLogin?"))
            {
                //var link = GetLoginLink();
                await HandleAuthentication();
            }

            Console.ReadKey();
            var emailCount = await HowManyMailsInInbox();
            Console.WriteLine(emailCount);

            Console.ReadKey();
            _scenarioFinishedEvent.Set();
        }

        private async Task<string> HowManyMailsInInbox()
        {
            var script =
            @"
               (function() {
                    var in_id = document.getElementById(':in');
                    var dj = in_id.getElementsByClassName('Dj')[0];

                    return dj.textContent;
                }
               )();
             ";

            var scriptExecution = await _browser.EvaluateScriptAsync(script);

            return (string)scriptExecution.Message;
        }

        private async Task<string> GetLoginLink()
        {
            var tcs = new TaskCompletionSource<bool>();

            var script =
            @"(function() {
                var id = 'gmail-sign-in';
                var loginElement = document.getElementById(id);
                var link = loginElement.getAttribute('href');
                return link;
               })();
             ";

            var response = await _browser.EvaluateScriptAsync(script);

            if (!response.Success)
            {
                Console.WriteLine(response.Message);
                return string.Empty;
            }
            else
                return (string)response.Result;

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

        private async Task HandleAuthentication()
        {
            var script_login =
            @"(function() {
                 var email_id = 'Email';
                 var next_id = 'next';
                 var email_input = document.getElementById(email_id);
                 email_input.value = 'username';
                 console.log('value =' + email_input.value);
                 var next_button = document.getElementById(next_id);
                 next_button.click();
               }
              )();";

            var scriptTask = await _browser.EvaluateScriptAsync(script_login);
            Thread.Sleep(1000);

            if (!scriptTask.Success)
                Console.WriteLine($"scriptTask failed {scriptTask.Message}");

            var script_password =
                    @"(function() {
                         var password_id = 'Passwd';
                         var connection_id = 'signIn';
                         var password_input = document.getElementById(password_id);
                         password_input.value = 'password';
                         console.log('value');
                         
                         //var connection_button = document.getElementById(connection_id);
                         //connection_button.click();

                         var form = document.getElementById('gaia_loginform');
                         form.submit();
                       }
                      )();";

            var signInScript = await _browser.EvaluateScriptAsync(script_password);
            if (!signInScript.Success)
                Console.WriteLine($"signInScript failed {signInScript.Message}");

            Thread.Sleep(1000);

                     // Wait for the screenshot to be taken.
            //var task = await _browser.ScreenshotAsync();
                     
            //             // Make a file to save it to (e.g. C:\Users\jan\Desktop\CefSharp screenshot.png)
            //var screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "CefSharp screenshot.png");

            //Console.WriteLine();
            //Console.WriteLine("Screenshot ready. Saving to {0}", screenshotPath);

            // // Save the Bitmap to the path.
            // // The image type is auto-detected via the ".png" extension.
            // //task.Result.Save(screenshotPath);

            // // We no longer need the Bitmap.
            // // Dispose it to avoid keeping the memory alive.  Especially important in 32-bit applications.
            // //task.Result.Dispose();

            // Console.WriteLine("Screenshot saved.  Launching your default image viewer...");

            // // Tell Windows to launch the saved image.
            // Process.Start(screenshotPath);
        }
    }
}
