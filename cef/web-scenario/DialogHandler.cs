using CefSharp;
using System;
using System.Collections.Generic;

namespace webscenario
{
    class DialogHandler : IDialogHandler
    {
        public bool OnFileDialog(IWebBrowser browserControl, IBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, List<string> acceptFilters, int selectedAcceptFilter, IFileDialogCallback callback)
        {
            Console.WriteLine("My DialogHandler opened");
            return true;
        }
    }

    class JSDialogHandler : IJsDialogHandler
    {
        public void OnDialogClosed(IWebBrowser browserControl, IBrowser browser)
        {
            throw new NotImplementedException();
        }

        public bool OnFileDialog(IWebBrowser browserControl, IBrowser browser, CefFileDialogMode mode, string title, string defaultFilePath, List<string> acceptFilters, int selectedAcceptFilter, IFileDialogCallback callback)
        {
            Console.WriteLine("My DialogHandler opened");
            return true;
        }

        public bool OnJSBeforeUnload(IWebBrowser browserControl, IBrowser browser, string message, bool isReload, IJsDialogCallback callback)
        {
            throw new NotImplementedException();
        }

        public bool OnJSDialog(IWebBrowser browserControl, IBrowser browser, string originUrl, CefJsDialogType dialogType, string messageText, string defaultPromptText, IJsDialogCallback callback, ref bool suppressMessage)
        {
            throw new NotImplementedException();
        }

        public void OnResetDialogState(IWebBrowser browserControl, IBrowser browser)
        {
            throw new NotImplementedException();
        }
    }
}
