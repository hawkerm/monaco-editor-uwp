using Collections.Generic;
using Monaco.Editor;
using Monaco.Extensions;
using Monaco.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    public sealed partial class CodeEditor : Control, INotifyPropertyChanged
    {
        private bool _initialized;
        private WebView _view;
        private ModelHelper _model;

        public event PropertyChangedEventHandler PropertyChanged;

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

        /// <summary>
        /// Construct a new IStandAloneCodeEditor.
        /// </summary>
        public CodeEditor()
        {
            this.DefaultStyleKey = typeof(CodeEditor);    
            
            if (this.Options != null)
            {
                // Set Pass-Thru Properties
                this.Options.GlyphMargin = this.HasGlyphMargin;
                this.Options.Language = this.CodeLanguage;

                // Register for changes
                this.Options.PropertyChanged += async (s, p) =>
                {
                    // TODO: Check for Language property and call other method instead?
                    await InvokeScriptAsync("updateOptions", s);
                };
            }

            // Initialize this here so property changed event will fire and register collection changed event.
            this.Decorations = new ObservableVector<IModelDeltaDecoration>();
            this.Markers = new ObservableVector<IMarkerData>();
            this._model = new ModelHelper(this);
        }

        protected override void OnApplyTemplate()
        {
            if (_view != null)
            {
                _view.NavigationStarting -= WebView_NavigationStarting;
                _view.DOMContentLoaded -= WebView_DOMContentLoaded;
                _view.NavigationCompleted -= WebView_NavigationCompleted;
                _view.NewWindowRequested -= WebView_NewWindowRequested;
                this._initialized = false;
            }

            _view = (WebView)GetTemplateChild("View");

            if (_view != null)
            {
                _view.NavigationStarting += WebView_NavigationStarting;
                _view.DOMContentLoaded += WebView_DOMContentLoaded;
                _view.NavigationCompleted += WebView_NavigationCompleted;
                _view.NewWindowRequested += WebView_NewWindowRequested;
                _view.Source = new Uri("ms-appx-web:///Monaco/MonacoEditor.html");
            }

            base.OnApplyTemplate();
        }

        internal async Task SendScriptAsync(string script,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await SendScriptAsync<object>(script, member, file, line);
        }

        internal async Task<T> SendScriptAsync<T>(string script, 
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            if (_initialized)
            {
                try
                {
                    return await this._view.RunScriptAsync<T>(script, member, file, line);
                }
                catch (Exception e)
                {
                    InternalException?.Invoke(this, e);
                }
            }
            else
            {
                #if DEBUG
                Debug.WriteLine("WARNING: Tried to call '" + script + "' before initialized.");
                #endif
            }

            return default(T);
        }

        internal async Task InvokeScriptAsync(
            string method,
            object arg,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await this.InvokeScriptAsync<object>(method, new object[] { arg }, serialize, member, file, line);
        }

        internal async Task InvokeScriptAsync(
            string method,
            object[] args,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await this.InvokeScriptAsync<object>(method, args, serialize, member, file, line);
        }

        internal async Task<T> InvokeScriptAsync<T>(
            string method,
            object arg,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            return await this.InvokeScriptAsync<T>(method, new object[] { arg }, serialize, member, file, line);
        }

        internal async Task<T> InvokeScriptAsync<T>(
            string method,
            object[] args,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            if (_initialized)
            {
                try
                {
                    return await this._view.InvokeScriptAsync<T>(method, args, serialize, member, file, line);
                }
                catch (Exception e)
                {
                    InternalException?.Invoke(this, e);
                }
            }
            else
            {
                #if DEBUG
                Debug.WriteLine("WARNING: Tried to call " + method + " before initialized.");
                #endif
            }

            return default(T);
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
