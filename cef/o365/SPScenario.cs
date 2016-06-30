using CefSharp;
using CefSharp.OffScreen;
using CefSharp.Internals;
using System;
using System.Threading.Tasks;

namespace o365
{
    class SPScenario
    {
        private string _url;
        private string _username;
        private string _password;

        private ChromiumWebBrowser _browser;

        SPScenario(string goTo, string username, string password)
        {
            _url = goTo;
            _username = username;
            _password = password;

            _browser = new ChromiumWebBrowser(_url);
        }

        public async void Run()
        {

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
    }
}
