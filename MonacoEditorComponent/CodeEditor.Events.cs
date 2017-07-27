using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Monaco
{
    public partial class CodeEditor
    {
        // Override default Loaded event so we can make sure we've initialized our WebView contents with the CodeEditor.
        public new event RoutedEventHandler Loaded;

        private ThemeListener _themeListener;

        private void WebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            Debug.WriteLine("DOM Content Loaded");
            this._initialized = true;
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            this.IsLoaded = true;
        }

        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Debug.WriteLine("Navigation Starting");
            var parent = new ParentAccessor(this);
            parent.RegisterAction("Loaded", () =>
            {
                Loaded?.Invoke(this, new RoutedEventArgs());
            });

            _themeListener = new ThemeListener();
            _themeListener.ThemeChanged += _themeListener_ThemeChanged;

            this._view.AddWebAllowedObject("Debug", new DebugLogger());
            this._view.AddWebAllowedObject("Parent", parent);
            this._view.AddWebAllowedObject("Theme", _themeListener);
        }

        private async void _themeListener_ThemeChanged(ThemeListener sender)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await this.InvokeScriptAsync("changeTheme", sender.CurrentTheme.ToString(), sender.IsHighContrast.ToString());
            });
        }
    }
}
