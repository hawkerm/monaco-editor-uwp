using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Monaco
{
    public partial class CodeEditor
    {
        // Override default Loaded/Loading event so we can make sure we've initialized our WebView contents with the CodeEditor.

        /// <summary>
        /// When Editor is Loading, it is ready to receive commands to the Monaco Engine.
        /// </summary>
        public new event RoutedEventHandler Loading;

        /// <summary>
        /// When Editor is Loaded, it has been rendered and is ready to be displayed.
        /// </summary>
        public new event RoutedEventHandler Loaded;

        /// <summary>
        /// Called when a link is Ctrl+Clicked on in the editor, set Handled to true to prevent opening.
        /// </summary>
        public event TypedEventHandler<WebView, WebViewNewWindowRequestedEventArgs> OpenLinkRequested;

        /// <summary>
        /// Custom Keyboard Handler.
        /// </summary>
        public new event WebKeyEventHandler KeyDown;

        private ThemeListener _themeListener;

        private void WebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            Debug.WriteLine("DOM Content Loaded");
            this._initialized = true;
        }

        private void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            this.IsLoaded = true;
            Loaded?.Invoke(this, new RoutedEventArgs());
        }

        private ParentAccessor _parentAccessor;

        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Debug.WriteLine("Navigation Starting");
            _parentAccessor = new ParentAccessor(this);
            _parentAccessor.RegisterAction("Loaded", () =>
            {
                Loading?.Invoke(this, new RoutedEventArgs());
            });

            _themeListener = new ThemeListener();
            _themeListener.ThemeChanged += _themeListener_ThemeChanged;

            this._view.AddWebAllowedObject("Debug", new DebugLogger());
            this._view.AddWebAllowedObject("Parent", _parentAccessor);
            this._view.AddWebAllowedObject("Theme", _themeListener);
            this._view.AddWebAllowedObject("Keyboard", new KeyboardListener(this));
        }

        private void WebView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            OpenLinkRequested?.Invoke(sender, args);
        }

        private async void _themeListener_ThemeChanged(ThemeListener sender)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await this.InvokeScriptAsync("changeTheme", sender.CurrentTheme.ToString(), sender.IsHighContrast.ToString());
            });
        }

        internal bool TriggerKeyDown(WebKeyEventArgs args)
        {
            this.KeyDown?.Invoke(this, args);

            return args.Handled;
        }
    }
}
