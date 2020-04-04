using Collections.Generic;
using Monaco.Editor;
using Monaco.Extensions;
using Monaco.Helpers;
using System;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;

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
        private Border _layoutRootBorder;

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
            _model = new ModelHelper(this);

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
            _model = new ModelHelper(this);

            _parentAccessor = new ParentAccessor(this);
            _parentAccessor.AddAssemblyForTypeLookup(typeof(Range).GetTypeInfo().Assembly);
            _parentAccessor.RegisterAction("Loaded", CodeEditorLoaded);

            _themeListener = new ThemeListener();
            _themeListener.ThemeChanged += ThemeListener_ThemeChanged;
            _themeToken = RegisterPropertyChangedCallback(RequestedThemeProperty, RequestedTheme_PropertyChanged);

            _keyboardListener = new KeyboardListener(this);

            _allowedObject.Clear();

            AddWebAllowedObject("Parent", _parentAccessor);
            AddWebAllowedObject("Theme", _themeListener);
            AddWebAllowedObject("Keyboard", _keyboardListener);

            Options.PropertyChanged += Options_PropertyChanged;

            Decorations.VectorChanged += Decorations_VectorChanged;
            Markers.VectorChanged += Markers_VectorChanged;

            //_view.NewWindowRequested += WebView_NewWindowRequested;

            await Task.Delay(100);

            _initialized = true;

            _view = new WebView2();
            _layoutRootBorder.Child = _view;

            _view.NavigationStarting += WebView_NavigationStarting;
            //_view.DOMContentLoaded += WebView_DOMContentLoaded;
            _view.NavigationCompleted += WebView_NavigationCompleted;
            //_view.NewWindowRequested += WebView_NewWindowRequested;
            _view.WebMessageReceived += WebView_WebMessageReceived;

            _view.UriSource = new System.Uri(@"file:///" + (await StorageFile.GetFileFromApplicationUriAsync(new System.Uri("ms-appx:///Monaco/CodeEditor/CodeEditor.html"))).Path);

            Loading?.Invoke(this, new RoutedEventArgs());

            Unloaded += CodeEditor_Unloaded;

            Loaded?.Invoke(this, new RoutedEventArgs());
        }

        private void CodeEditor_Unloaded(object sender, RoutedEventArgs e)
        {
            Unloaded -= CodeEditor_Unloaded;

            if (_view != null)
            {
                _view.NavigationStarting -= WebView_NavigationStarting;
                //_view.DOMContentLoaded -= WebView_DOMContentLoaded;
                _view.NavigationCompleted -= WebView_NavigationCompleted;
                //_view.NewWindowRequested -= WebView_NewWindowRequested;
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
                _view.NavigationStarting -= WebView_NavigationStarting;
                //_view.DOMContentLoaded -= WebView_DOMContentLoaded;
                _view.NavigationCompleted -= WebView_NavigationCompleted;
                //_view.NewWindowRequested -= WebView_NewWindowRequested;
                _view.WebMessageReceived -= WebView_WebMessageReceived;
                _initialized = false;
            }

            _layoutRootBorder = (Border)GetTemplateChild("LayoutRootBorder");
            /*<controls:WebView2 x:Name="View"
                Margin="{TemplateBinding Padding}"
                Visibility="{Binding IsLoaded,RelativeSource={RelativeSource TemplatedParent}}"
                HorizontalAlignment="Stretch"
                VerticalAlignment="Stretch"/>
                */

            //_view = (WebView2)GetTemplateChild("View");

            //base.OnApplyTemplate();

            //if (_view != null)
            //{
            //    _view.NavigationStarting += WebView_NavigationStarting;
            //    //_view.DOMContentLoaded += WebView_DOMContentLoaded;
            //    _view.NavigationCompleted += WebView_NavigationCompleted;
            //    //_view.NewWindowRequested += WebView_NewWindowRequested;
            //    _view.WebMessageReceived += WebView_WebMessageReceived;
            //    _view.UriSource = new Uri("ms-appx-web:///Monaco/MonacoEditor.html");
            //}
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
