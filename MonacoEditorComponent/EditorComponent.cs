using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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
    }
}
