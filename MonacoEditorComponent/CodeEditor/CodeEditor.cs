﻿using Collections.Generic;
using Monaco.Editor;
using Monaco.Extensions;
using Monaco.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel;
using System.IO;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Monaco
{
    /// <summary>
    /// UWP Windows Runtime Component wrapper for the Monaco CodeEditor
    /// https://microsoft.github.io/monaco-editor/
    /// </summary>
    [TemplatePart(Name = "View", Type = typeof(WebView2))]
    public sealed partial class CodeEditor : Control, INotifyPropertyChanged, IDisposable
    {
        private bool _initialized;
        private WebView2 _view;
        private ModelHelper _model;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Template Property used during loading to prevent blank control visibility when it's still loading WebView.
        /// </summary>
        public bool IsEditorLoaded
        {
            get => (bool)GetValue(IsEditorLoadedProperty);
            private set => SetValue(IsEditorLoadedProperty, value);
        }

        public static DependencyProperty IsEditorLoadedProperty { get; } = DependencyProperty.Register(nameof(IsEditorLoaded), typeof(string), typeof(CodeEditor), new PropertyMetadata(false));

        /// <summary>
        /// Construct a new IStandAloneCodeEditor.
        /// </summary>
        public CodeEditor()
        {
            DefaultStyleKey = typeof(CodeEditor);
            if (Options != null)
            {
                // Set Pass-Thru Properties
                Options.GlyphMargin = HasGlyphMargin;

                // Register for changes
                Options.PropertyChanged += Options_PropertyChanged;
            }

            // Initialize this here so property changed event will fire and register collection changed event.
            Decorations = new ObservableVector<IModelDeltaDecoration>();
            Markers = new ObservableVector<IMarkerData>();

            base.Loaded += CodeEditor_Loaded;
            Unloaded += CodeEditor_Unloaded;
        }

        private async void Options_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!(sender is StandaloneEditorConstructionOptions options)) return;
            if (e.PropertyName == nameof(StandaloneEditorConstructionOptions.Language))
            {
                await ExecuteScriptAsync("updateLanguage", options.Language);
                if (CodeLanguage != options.Language) CodeLanguage = options.Language;
            }
            if (e.PropertyName == nameof(StandaloneEditorConstructionOptions.GlyphMargin))
            {
                if (HasGlyphMargin != options.GlyphMargin) options.GlyphMargin = HasGlyphMargin;
            }
            if (e.PropertyName == nameof(StandaloneEditorConstructionOptions.ReadOnly))
            {
                if (ReadOnly != options.ReadOnly) options.ReadOnly = ReadOnly;
            }
            await ExecuteScriptAsync("updateOptions", options);
        }

        private async void CodeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            // Do this the 2nd time around.
            if (_model == null && _view != null)
            {
                _model = new ModelHelper(this);

                Options.PropertyChanged += Options_PropertyChanged;

                Decorations.VectorChanged += Decorations_VectorChanged;
                Markers.VectorChanged += Markers_VectorChanged;

                await _view.EnsureCoreWebView2Async();

                if (_view.CoreWebView2 != null)
                {
                    _initializedTcs = new TaskCompletionSource<ulong>();
                    _view.CoreWebView2.NewWindowRequested += WebView_NewWindowRequested;
                    _view.CoreWebView2.DOMContentLoaded += WebView_DOMContentLoaded;
                }

                SetWebViewSource();

                await _initializedTcs.Task;
                _initializedTcs = null;

                Loading?.Invoke(this, new RoutedEventArgs());

                Unloaded -= CodeEditor_Unloaded;
                Unloaded += CodeEditor_Unloaded;

                Loaded?.Invoke(this, new RoutedEventArgs());
            }
        }

        private void CodeEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= CodeEditor_Unloaded;

            if (_view != null)
            {
                _view.CoreProcessFailed -= WebView_CoreProcessFailed;
                _view.NavigationStarting -= WebView_NavigationStarting;
                _view.NavigationCompleted -= WebView_NavigationCompleted;
                _view.CoreWebView2Initialized -= WebView_CoreWebView2Initialized;
                if (_view.CoreWebView2 != null)
                {
                    _view.CoreWebView2.NewWindowRequested -= WebView_NewWindowRequested;
                    _view.CoreWebView2.DOMContentLoaded -= WebView_DOMContentLoaded;
                }
                _view.WebMessageReceived -= WebView_WebMessageReceived;
                _initialized = false;
            }

            Decorations.VectorChanged -= Decorations_VectorChanged;
            Markers.VectorChanged -= Markers_VectorChanged;

            Options.PropertyChanged -= Options_PropertyChanged;

            if (_themeListener != null)
            {
                _themeListener.ThemeChanged -= ThemeListener_ThemeChanged;
            }
            _themeListener = null;

            UnregisterPropertyChangedCallback(RequestedThemeProperty, _themeToken);
            _keyboardListener = null;
            _model = null;
        }

        protected override void OnApplyTemplate()
        {
            if (_view != null)
            {
                _view.CoreProcessFailed -= WebView_CoreProcessFailed;
                _view.NavigationStarting -= WebView_NavigationStarting;
                if (_view.CoreWebView2 != null)
                {
                    _view.CoreWebView2.NewWindowRequested -= WebView_NewWindowRequested;
                    _view.CoreWebView2.DOMContentLoaded -= WebView_DOMContentLoaded;
                }
                _view.NavigationCompleted -= WebView_NavigationCompleted;
                _view.CoreWebView2Initialized -= WebView_CoreWebView2Initialized;
                _view.WebMessageReceived -= WebView_WebMessageReceived;
                _initialized = false;
            }

            _view = (WebView2)GetTemplateChild("View");

            if (_view != null)
            {
                _view.CoreProcessFailed += WebView_CoreProcessFailed;
                _view.NavigationStarting += WebView_NavigationStarting;
                _view.NavigationCompleted += WebView_NavigationCompleted;
                _view.CoreWebView2Initialized += WebView_CoreWebView2Initialized;
                _view.WebMessageReceived += WebView_WebMessageReceived;
            }

            base.OnApplyTemplate();
        }

        private void WebView_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            _view.CoreWebView2.DOMContentLoaded -= WebView_DOMContentLoaded;
            _view.CoreWebView2.NewWindowRequested -= WebView_NewWindowRequested;

            _view.CoreWebView2.DOMContentLoaded += WebView_DOMContentLoaded;
            _view.CoreWebView2.NewWindowRequested += WebView_NewWindowRequested;
        }

        private void SetWebViewSource()
        {
            _view.Source = new System.Uri(@"file:///" + Path.Combine(Package.Current.InstalledLocation.Path, @"MonacoEditorComponent/CodeEditor/CodeEditor.html"));
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
                    return await _view.RunScriptAsync<T>(script, member, file, line);
                }
                catch (Exception e)
                {
                    InternalException?.Invoke(this, e);
                }
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("WARNING: Tried to call '" + script + "' before initialized.");
#endif
            }

            return default;
        }

        internal async Task ExecuteScriptAsync(
            string method,
            object arg,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await ExecuteScriptAsync<object>(method, new object[] { arg }, serialize, member, file, line);
        }

        internal async Task ExecuteScriptAsync(
            string method,
            object[] args,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            await ExecuteScriptAsync<object>(method, args, serialize, member, file, line);
        }

        internal async Task<T> ExecuteScriptAsync<T>(
            string method,
            object arg,
            bool serialize = true,
            [CallerMemberName] string member = null,
            [CallerFilePath] string file = null,
            [CallerLineNumber] int line = 0)
        {
            return await ExecuteScriptAsync<T>(method, new object[] { arg }, serialize, member, file, line);
        }

        internal async Task<T> ExecuteScriptAsync<T>(
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
                    return await _view.ExecuteScriptAsync<T>(method, args, serialize, member, file, line);
                }
                catch (Exception e)
                {
                    InternalException?.Invoke(this, e);
                }
            }
            else
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("WARNING: Tried to call " + method + " before initialized.");
#endif
            }

            return default;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _parentAccessor?.Dispose();
            _parentAccessor = null;
            CssStyleBroker.DetachEditor(this);
        }
    }
}
