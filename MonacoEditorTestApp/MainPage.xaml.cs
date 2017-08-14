using Monaco;
using Monaco.Editor;
using Monaco.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonacoEditorTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string[] _decorations = Array.Empty<string>();

        public string CodeContent
        {
            get { return (string)GetValue(CodeContentProperty); }
            set { SetValue(CodeContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeContentProperty =
            DependencyProperty.Register("CodeContent", typeof(string), typeof(MainPage), new PropertyMetadata(""));

        public MainPage()
        {
            this.CodeContent = "public class Program {\n\tpublic static void Main(string[] args) {\n\t\tConsole.WriteLine(\"Hello, World!\");\n\t}\n}";

            this.InitializeComponent();

            Editor.Loading += Editor_Loading;
            Editor.Loaded += Editor_Loaded;
        }

        private async void Editor_Loading(object sender, RoutedEventArgs e)
        {
            // Ready for Code
            var languages = await new Monaco.LanguagesHelper(Editor).GetLanguagesAsync();
            //Debugger.Break();
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            // Ready for Display
        }

        private void ButtonSetText_Click(object sender, RoutedEventArgs e)
        {
            CodeContent = TextEditor.Text;
        }

        private async void ButtonRevealPositionInCenter_Click(object sender, RoutedEventArgs e)
        {
            await this.Editor.RevealPositionInCenterAsync(new Monaco.Position(10, 5));
        }

        private async void ButtonHighlightRange_Click(object sender, RoutedEventArgs e)
        {
            this._decorations = (await this.Editor.DeltaDecorationsAsync(this._decorations, new IModelDeltaDecoration[] {
                new IModelDeltaDecoration(new Range(3,1,3,10), new IModelDecorationOptions() {
                    ClassName = new CssLineStyle() // TODO: Save these styles so we don't keep regenerating them and adding new ones.
                    {
                        BackgroundColor = new SolidColorBrush(Colors.Red)
                    },
                    HoverMessage = new string[]
                    {
                        "This is a test message.",
                        "*YES*, **it is**."
                    }
                })
            })).ToArray();
        }

        private async void ButtonHighlightLine_Click(object sender, RoutedEventArgs e)
        {
            this._decorations = (await this.Editor.DeltaDecorationsAsync(this._decorations, new IModelDeltaDecoration[] {
                new IModelDeltaDecoration(new Range(4,1,4,1), new IModelDecorationOptions() {
                    IsWholeLine = true,
                    ClassName = new CssLineStyle()
                    {
                        BackgroundColor = new SolidColorBrush(Colors.AliceBlue)
                    },
                    GlyphMarginClassName = new CssGlyphStyle()
                    {
                        GlyphImage = new Uri("ms-appx-web:///Icons/error.png")
                    },
                    HoverMessage = new string[]
                    {
                        "This is *another* test message."
                    },
                    GlyphMarginHoverMessage = new string []
                    {
                        "This is some crazy Error here.",
                        "Maybe..."
                    }
                }),
                new IModelDeltaDecoration(new Range(2, 1, 2, 1), new IModelDecorationOptions()
                {
                    IsWholeLine = true,
                    InlineClassName = new CssInlineStyle()
                    {
                        TextDecoration = TextDecoration.LineThrough
                    },
                    HoverMessage = new string[]
                    {
                        "Deprecated"
                    }
                })
            })).ToArray();
        }

        private async void ButtonClearHighlights_Click(object sender, RoutedEventArgs e)
        {
            _decorations = (await this.Editor.DeltaDecorationsAsync(this._decorations, Array.Empty<IModelDeltaDecoration>())).ToArray();
            _decorations = Array.Empty<string>();
        }

        private void Editor_KeyDown(object sender, WebKeyEventArgs e)
        {
            Debug.WriteLine("KeyDown: " + e.KeyCode + " " + e.CtrlKey);

            if (e.KeyCode == 112) // F1
            {
                e.Handled = true;
            } else if (e.KeyCode == 13 && e.CtrlKey)
            {
                Debug.WriteLine("Execute");
                e.Handled = true;
            }
        }

        private void ButtonFolding_Click(object sender, RoutedEventArgs e)
        {
            Editor.Options.Folding = !Editor.Options.Folding ?? true;
        }

        private void ButtonMinimap_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Need to propagate the INotifyPropertyChanged from the Sub-Option Objects
            Editor.Options.Minimap = new IEditorMinimapOptions()
            {
                Enabled = !Editor.Options.Minimap?.Enabled ?? false
            };
        }

        private void ButtonChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            Editor.CodeLanguage = (Editor.CodeLanguage == "csharp") ? "xml" : "csharp";
        }
    }
}
