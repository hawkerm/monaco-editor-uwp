using Monaco.Helpers;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Monaco
{
    /// <summary>
    /// UWP Windows Runtime Component wrapper for the Monaco CodeEditor
    /// https://microsoft.github.io/monaco-editor/
    /// </summary>
    [TemplatePart(Name = "View", Type = typeof(WebView))]
    public sealed partial class CodeEditor : Control
    {
        private bool _initialized;
        private WebView _view;        

        /// <summary>
        /// Template Property used during loading to prevent blank control visibility when it's still loading WebView.
        /// </summary>
        public bool IsLoaded
        {
            get { return (bool)GetValue(IsLoadedProperty); }
            private set { SetValue(IsLoadedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsLoadedPropertyField =
            DependencyProperty.Register("IsLoaded", typeof(string), typeof(CodeEditor), new PropertyMetadata(false));

        public static DependencyProperty IsLoadedProperty
        {
            get
            {
                return IsLoadedPropertyField;
            }
        }

        public CodeEditor()
        {
            this.DefaultStyleKey = typeof(CodeEditor);            
        }

        protected override void OnApplyTemplate()
        {
            if (_view != null)
            {
                _view.NavigationStarting -= WebView_NavigationStarting;
                _view.DOMContentLoaded -= WebView_DOMContentLoaded;
                _view.NavigationCompleted -= WebView_NavigationCompleted;
                this._initialized = false;
            }

            _view = (WebView)GetTemplateChild("View");

            if (_view != null)
            {
                _view.NavigationStarting += WebView_NavigationStarting;
                _view.DOMContentLoaded += WebView_DOMContentLoaded;
                _view.NavigationCompleted += WebView_NavigationCompleted;
                _view.Source = new Uri("ms-appx-web:///Monaco/MonacoEditor.html");
            }

            base.OnApplyTemplate();
        }

        internal async Task<string> SendScriptAsync(string script)
        {
            if (_initialized)
            {
                if (Dispatcher.HasThreadAccess)
                {
                    return await this._view.InvokeScriptAsync("eval", new string[] { script });
                }
                else
                {
                    return await Dispatcher.RunTaskAsync(async () => {
                        return await _view.InvokeScriptAsync("eval", new string[] { script });
                    });
                }                
            }

            return string.Empty;
        }

        internal async Task<string> InvokeScriptAsync(string method, params string[] args)
        {
            if (_initialized)
            {
                if (Dispatcher.HasThreadAccess)
                {
                    return await this._view.InvokeScriptAsync(method, args);
                }
                else
                {
                    return await Dispatcher.RunTaskAsync(async () =>
                    {
                        return await this._view.InvokeScriptAsync(method, args);
                    });
                }
            }

            return string.Empty;
        }
    }
}
