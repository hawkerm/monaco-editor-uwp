using Monaco;
using Monaco.Editor;
using Monaco.Helpers;
using MonacoEditorTestApp.Actions;
using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonacoEditorTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string CodeContent
        {
            get { return (string)GetValue(CodeContentProperty); }
            set { SetValue(CodeContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeContentProperty =
            DependencyProperty.Register("CodeContent", typeof(string), typeof(MainPage), new PropertyMetadata(""));

        private ContextKey _myCondition;

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

            _myCondition = await Editor.CreateContextKeyAsync("MyCondition", false);

            await Editor.AddCommandAsync(Monaco.KeyCode.F5, async () => {
                var md = new MessageDialog("You Hit F5!");
                await md.ShowAsync();

                // Turn off Command again.
                _myCondition.Reset();

                // Refocus on CodeEditor, Note: Issue #7
                Editor.Focus(FocusState.Programmatic);
            }, _myCondition.Key);

            await Editor.AddCommandAsync(Monaco.KeyMod.CtrlCmd | Monaco.KeyCode.KEY_W, async () =>
            {
                var word = await Editor.GetModel().GetWordAtPositionAsync(await Editor.GetPositionAsync());

                var md = new MessageDialog("Word: " + word.Word + "[" + word.StartColumn + ", " + word.EndColumn + "]");
                await md.ShowAsync();
            });

            await Editor.AddCommandAsync(Monaco.KeyMod.CtrlCmd | Monaco.KeyCode.KEY_L, async () =>
            {
                var model = Editor.GetModel();
                var line = await model.GetLineContentAsync((await Editor.GetPositionAsync()).LineNumber);
                var lines = await model.GetLinesContentAsync();
                var count = await model.GetLineCountAsync();

                var md = new MessageDialog("Current Line: " + line + "\nAll Lines [" + count + "]:\n" + string.Join("\n", lines));
                await md.ShowAsync();
            });

            await Editor.AddCommandAsync(Monaco.KeyMod.CtrlCmd | Monaco.KeyCode.KEY_U, async () =>
            {
                var range = new Range(2, 10, 3, 8);
                var seg = await Editor.GetModel().GetValueInRangeAsync(range);

                var md = new MessageDialog("Segment " + range.ToString() + ": " + seg);
                await md.ShowAsync();
            });

            await Editor.AddActionAsync(new TestAction());
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

        private void ButtonHighlightRange_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Decorations.Add(
                new IModelDeltaDecoration(new Range(3, 1, 3, 10), new IModelDecorationOptions()
                {
                    ClassName = new CssLineStyle() // TODO: Save these styles so we don't keep regenerating them and adding new ones.
                    {
                        BackgroundColor = new SolidColorBrush(Colors.Red)
                    },
                    HoverMessage = new string[]
                    {
                        "This is a test message.",
                        "*YES*, **it is**."
                    }
                }));
        }

        private void ButtonHighlightLine_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Decorations.Add(
                new IModelDeltaDecoration(new Range(4, 1, 4, 1), new IModelDecorationOptions() {
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
                    GlyphMarginHoverMessage = new string[]
                    {
                        "This is some crazy Error here.",
                        "Maybe..."
                    }
                }));
            this.Editor.Decorations.Add(
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
                }));
        }

        private void ButtonClearHighlights_Click(object sender, RoutedEventArgs e)
        {
            this.Editor.Decorations.Clear();
        }

        // Note: Can't make this method async as otherwise handled won't be read for intercepts.
        private void Editor_KeyDown(object sender, WebKeyEventArgs e)
        {
            Debug.WriteLine("KeyDown: " + e.KeyCode + " " + e.CtrlKey);

            if (e.KeyCode == 112) // F1
            {
                // If we wanted to disable the Command Palette (F1), we set handled to true here.
                //e.Handled = true;
            } else if (e.KeyCode == 13 && e.CtrlKey)
            {
                // You can now do this with a Command as well, see above.

                // Skip await, so we can read intercept value.
                #pragma warning disable CS4014
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                {
                    var md = new MessageDialog("You Hit Ctrl+Enter!");
                    await md.ShowAsync();

                    // Refocus on CodeEditor, Note: Issue #7
                    Editor.Focus(FocusState.Programmatic);
                });
                #pragma warning restore CS4014

                // Intercept input so we don't add a newline.
                e.Handled = true;

                // We'll show that we can enable the F5 Command once we've performed Ctrl+Enter at least once.
                _myCondition.Set(true);
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
