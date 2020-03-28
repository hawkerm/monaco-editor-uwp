using Monaco.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        /// Called when an internal exception is encountered while executing a command. (for testing/reporting issues)
        /// </summary>
        public event TypedEventHandler<CodeEditor, Exception> InternalException;

        /// <summary>
        /// Custom Keyboard Handler.
        /// </summary>
        public new event WebKeyEventHandler KeyDown;

        private ThemeListener _themeListener;

        private void WebView_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            #if DEBUG
            Debug.WriteLine("DOM Content Loaded");
            #endif
            _initialized = true;
        }

        private async void WebView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            IsEditorLoaded = true;

            // Make sure inner editor is focused
            await SendScriptAsync("editor.focus();");

            // If we're supposed to have focus, make sure we try and refocus on our now loaded webview.
            if (FocusManager.GetFocusedElement() == this)
            {
                _view.Focus(FocusState.Programmatic);
            }

            Loaded?.Invoke(this, new RoutedEventArgs());
        }

        internal ParentAccessor _parentAccessor;
        private KeyboardListener _keyboardListener;
        private long _themeToken;

        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            #if DEBUG
            Debug.WriteLine("Navigation Starting");
            #endif
            _parentAccessor = new ParentAccessor(this);
            _parentAccessor.AddAssemblyForTypeLookup(typeof(Range).GetTypeInfo().Assembly);
            _parentAccessor.RegisterAction("Loaded", CodeEditorLoaded);

            _themeListener = new ThemeListener();
            _themeListener.ThemeChanged += ThemeListener_ThemeChanged;
            _themeToken = RegisterPropertyChangedCallback(RequestedThemeProperty, RequestedTheme_PropertyChanged);

            _keyboardListener = new KeyboardListener(this);

            _view.AddWebAllowedObject("Debug", new DebugLogger());
            _view.AddWebAllowedObject("Parent", _parentAccessor);
            _view.AddWebAllowedObject("Theme", _themeListener);
            _view.AddWebAllowedObject("Keyboard", _keyboardListener);
        }        

        private async void CodeEditorLoaded()
        {
            if (Decorations != null && Decorations.Count > 0)
            {
                // Need to retrigger highlights after load if they were set before load.
                await DeltaDecorationsHelperAsync(Decorations.ToArray());
            }

            // Now we're done loading
            Loading?.Invoke(this, new RoutedEventArgs());
        }

        private void WebView_NewWindowRequested(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            // TODO: Should probably create own event args here as we don't want to expose the referrer to our internal page?
            OpenLinkRequested?.Invoke(sender, args);
        }

        private async void RequestedTheme_PropertyChanged(DependencyObject obj, DependencyProperty property)
        {
            var editor = obj as CodeEditor;
            var theme = editor.RequestedTheme;
            var tstr = string.Empty;

            if (theme == ElementTheme.Default)
            {
                tstr = _themeListener.CurrentThemeName;
            }
            else
            {
                tstr = theme.ToString();
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await InvokeScriptAsync("changeTheme", new string[] { tstr, _themeListener.IsHighContrast.ToString() });
            });
        }

        private async void ThemeListener_ThemeChanged(ThemeListener sender)
        {
            if (RequestedTheme == ElementTheme.Default)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    await InvokeScriptAsync("changeTheme", args: new string[] { sender.CurrentTheme.ToString(), sender.IsHighContrast.ToString() });
                });
            }
        }

        internal bool TriggerKeyDown(WebKeyEventArgs args)
        {
            KeyDown?.Invoke(this, args);

            return args.Handled;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            if (_view != null && FocusManager.GetFocusedElement() == this)
            {
                // Forward Focus onto our inner WebView
                _view.Focus(FocusState.Programmatic);
            }
        }
    }
}
