using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Monaco
{
    [TemplatePart(Name = "View", Type = typeof(WebView))]
    public sealed partial class EditorComponent : Control
    {
        private bool _initialized;
        private WebView _view;

        public EditorComponent()
        {
            this.DefaultStyleKey = typeof(EditorComponent);            
        }

        protected override void OnApplyTemplate()
        {
            if (_view != null)
            {
                _view.NavigationStarting -= WebView_NavigationStarting;
                _view.DOMContentLoaded -= WebView_DOMContentLoaded;
                this._initialized = false;
            }

            _view = (WebView)GetTemplateChild("View");

            if (_view != null)
            {
                _view.NavigationStarting += WebView_NavigationStarting;
                _view.DOMContentLoaded += WebView_DOMContentLoaded;
                _view.Source = new Uri("ms-appx-web:///Monaco/MonacoEditor.html");
            }

            base.OnApplyTemplate();
        }

        internal async Task<string> SendScriptAsync(string script)
        {
            if (_initialized)
            {
                return await this._view.InvokeScriptAsync("eval", new string[] { script });
            }

            return string.Empty;
        }

        internal async Task<string> InvokeScriptAsync(string method, params string[] args)
        {
            if (_initialized)
            {
                return await this._view.InvokeScriptAsync(method, args);
            }

            return string.Empty;
        }
    }
}
