using Monaco;
using Monaco.Editor;
using Monaco.Helpers;
using MonacoEditorTestApp.Actions;
using MonacoEditorTestApp.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;
using Microsoft.UI;
using Windows.UI.Popups;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.System;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonacoEditorTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly StandaloneEditorConstructionOptions options;
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
            InitializeComponent();
            options = Editor.Options;
            Editor.Loading += Editor_Loading;
            Editor.Loaded += Editor_Loaded;
            //Editor.OpenLinkRequested += Editor_OpenLinkRequest;

            Editor.InternalException += Editor_InternalException;
        }

        private void Editor_InternalException(CodeEditor sender, Exception args)
        {
            // This shouldn't happen, if it does, then it's a bug.
        }


        private async void Editor_Loading(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CodeContent))
            {
                CodeContent = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new System.Uri("ms-appx:///Content.txt")));

                ButtonHighlightRange_Click(null, null);
            }

            // Ready for Code
            var languages = new Monaco.LanguagesHelper(Editor);

            var available_languages = await languages.GetLanguagesAsync();
            //Debugger.Break();

            await languages.RegisterHoverProviderAsync("csharp", new EditorHoverProvider());

            await languages.RegisterCompletionItemProviderAsync("csharp", new LanguageProvider());

            _myCondition = await Editor.CreateContextKeyAsync("MyCondition", false);

            await Editor.AddCommandAsync(KeyCode.F5, async () => {
                var md = new MessageDialog("You Hit F5!");
                await md.ShowAsync();

                // Turn off Command again.
                _myCondition?.Reset();

                // Refocus on CodeEditor
                Editor.Focus(FocusState.Programmatic);
            }, _myCondition.Key);

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_R, async () =>
            {
                var range = await Editor.GetModel().GetFullModelRangeAsync();

                var md = new MessageDialog("Document Range: " + range.ToString());
                await md.ShowAsync();

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_W, async () =>
            {
                var word = await Editor.GetModel().GetWordAtPositionAsync(await Editor.GetPositionAsync());

                if (word == null)
                {
                    var md = new MessageDialog("No Word Found.");
                    await md.ShowAsync();
                }
                else
                {
                    var md = new MessageDialog("Word: " + word.Word + "[" + word.StartColumn + ", " + word.EndColumn + "]");
                    await md.ShowAsync();
                }

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_L, async () =>
            {
                var model = Editor.GetModel();
                var line = await model.GetLineContentAsync((await Editor.GetPositionAsync()).LineNumber);
                var lines = await model.GetLinesContentAsync();
                var count = await model.GetLineCountAsync();

                var md = new MessageDialog("Current Line: " + line + "\nAll Lines [" + count + "]:\n" + string.Join("\n", lines));
                await md.ShowAsync();

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_U, async () =>
            {
                var range = new Range(2, 10, 3, 8);
                var seg = await Editor.GetModel().GetValueInRangeAsync(range);

                var md = new MessageDialog("Segment " + range.ToString() + ": " + seg);
                await md.ShowAsync();

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddActionAsync(new TestAction());
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            // Ready for Display
        }

        //private void Editor_OpenLinkRequest(WebView2 sender, WebView2NewWindowRequestedEventArgs args)
        //{
        //    if (this.AllowWeb.IsChecked == false)
        //    {
        //        args.Handled = true;
        //    }
        //}

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
                    ClassName = new CssLineStyle(Editor) // TODO: Save these styles so we don't keep regenerating them and adding new ones.
                    {
                        BackgroundColor = new SolidColorBrush(Colors.DarkRed),
                    },
                    InlineClassName = new CssInlineStyle(Editor)
                    {
                        ForegroundColor = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        FontStyle = Windows.UI.Text.FontStyle.Italic
                    },
                    HoverMessage = new string[]
                    {
                        "This is a test message.",
                        "*YES*, **it is**."
                    }.ToMarkdownString(),
                    Stickiness = TrackedRangeStickiness.NeverGrowsWhenTypingAtEdges
                }));
        }

        private async void ButtonHighlightLine_Click(object sender, RoutedEventArgs e)
        {
            Editor.Decorations.Add(
                new IModelDeltaDecoration(new Range(4, 1, 4, 1), new IModelDecorationOptions() {
                    IsWholeLine = true,
                    ClassName = new CssLineStyle(Editor)
                    {
                        BackgroundColor = new SolidColorBrush(Colors.AliceBlue)
                    },
                    GlyphMarginClassName = new CssGlyphStyle(Editor)
                    {
                        GlyphImage = new System.Uri("ms-appx-web:///Icons/error.png")
                    },
                    HoverMessage = (new string[]
                    {
                        "This is *another* \"test\" message about 'thing'."
                    }).ToMarkdownString(),
                    GlyphMarginHoverMessage = (new string[]
                    {
                        "This is some crazy \"Error\" here.",
                        "'Maybe'..."
                    }).ToMarkdownString()
                }));
            Editor.Decorations.Add(
                new IModelDeltaDecoration(new Range(2, 1, 2, await Editor.GetModel().GetLineLengthAsync(2)), new IModelDecorationOptions()
                {
                    IsWholeLine = true,
                    InlineClassName = new CssInlineStyle(Editor)
                    {
                        TextDecoration = TextDecoration.LineThrough
                    },
                    GlyphMarginClassName = new CssGlyphStyle(Editor)
                    {
                        GlyphImage = new System.Uri("ms-appx-web:///Icons/warning.png")
                    },
                    HoverMessage = (new string[]
                    {
                        "Deprecated"
                    }).ToMarkdownString()
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
                DispatcherQueue.GetForCurrentThread().TryEnqueue(DispatcherQueuePriority.Low, async () =>
                {
                    var md = new MessageDialog("You Hit Ctrl+Enter!");
                    await md.ShowAsync();

                    // Refocus on CodeEditor
                    Editor.Focus(FocusState.Programmatic);
                });
                #pragma warning restore CS4014

                // Intercept input so we don't add a newline.
                e.Handled = true;

                // We'll show that we can enable the F5 Command once we've performed Ctrl+Enter at least once.
                _myCondition?.Set(true);
            }
        }

        private void ButtonFolding_Click(object sender, RoutedEventArgs e)
        {
            options.Folding = !options.Folding ?? true;
        }

        private void ButtonMinimap_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Need to propagate the INotifyPropertyChanged from the Sub-Option Objects
            options.Minimap = new EditorMinimapOptions()
            {
                Enabled = !options.Minimap?.Enabled ?? false
            };
        }

        private void ButtonChangeLanguage_Click(object sender, RoutedEventArgs e)
        {
            Editor.CodeLanguage = (Editor.CodeLanguage == "csharp") ? "xml" : "csharp";
        }

        private async void ButtonSetMarker_Click(object sender, RoutedEventArgs e)
        {
            if ((await Editor.GetModelMarkersAsync()).Count() == 0)
            {
                Editor.Markers.Add(
                    new MarkerData()
                    {
                        Code = "2344",
                        Message = "This is a \"Warning\" about 'that thing'.",
                        Severity = MarkerSeverity.Warning,
                        Source = "Origin",
                        StartLineNumber = 2,
                        StartColumn = 2,
                        EndLineNumber = 2,
                        EndColumn = 8
                    });

                Editor.Markers.Add(
                    new MarkerData()
                    {
                        Code = "2345",
                        Message = "This is an \"Error\" about 'that thing'.",
                        Severity = MarkerSeverity.Error,
                        Source = "Origin",
                        StartLineNumber = 3,
                        StartColumn = 5,
                        EndLineNumber = 3,
                        EndColumn = 15
                    });
            }
            else
            {
                //Editor.Markers.Clear();
                await Editor.SetModelMarkersAsync("CodeEditor", Array.Empty<IMarkerData>());
            }            
        }

        //// Example to show toggling visibility and impact on control.
        private void HideButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn.Content.ToString() == "Hide")
            {
                Editor.Visibility = Visibility.Collapsed;

                btn.Content = "Show";
            }
            else
            {
                Editor.Visibility = Visibility.Visible;

                btn.Content = "Hide";
            }
        }

        // TODO: this scenario needs more work.
        //// Example to show keeping a reference to the editor but removing from Visual Tree.
        private void DetachButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn.Content.ToString() == "Detach")
            {
                RootGrid.Children.Remove(Editor);

                GC.Collect();
                GC.WaitForPendingFinalizers();

                btn.Content = "Attach";
            }
            else
            {
                RootGrid.Children.Add(Editor);

                btn.Content = "Detach";
            }
        }

        //// Example to show memory usage when deconstructing and reconstructing editor.
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            if (btn.Content.ToString() == "Remove")
            {
                _myCondition = null;
                Editor.KeyDown -= Editor_KeyDown;

                Editor.Loaded -= Editor_Loaded;
                Editor.Loading -= Editor_Loading;
                //Editor.OpenLinkRequested -= Editor_OpenLinkRequest;
                Editor.InternalException -= Editor_InternalException;

                RootGrid.Children.Remove(Editor);
                Editor.Dispose();
                Editor = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                btn.Content = "Add";
            }
            else
            {
                Editor = new CodeEditor()
                {
                    TabIndex = 0,
                    HasGlyphMargin = true,
                    CodeLanguage = "csharp"
                };

                Editor.KeyDown += Editor_KeyDown;

                Editor.Loading += Editor_Loading;
                Editor.Loaded += Editor_Loaded;
                //Editor.OpenLinkRequested += Editor_OpenLinkRequest;
                Editor.InternalException += Editor_InternalException;

                Grid.SetColumn(Editor, 1);

                RootGrid.Children.Add(Editor);

                // TODO: My Condition?

                btn.Content = "Remove";
            }           
        }

        private void ComboBoxTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch(e.AddedItems.FirstOrDefault().ToString())
            {
                case "System":
                    RequestedTheme = ElementTheme.Default;
                    break;
                case "Light":
                    RequestedTheme = ElementTheme.Light;
                    break;
                case "Dark":
                    RequestedTheme = ElementTheme.Dark;
                    break;
            }

            // Tell Editor about Update.
            Editor.RequestedTheme = RequestedTheme;
        }

        private async void LoadAndSet_Click(object sender, RoutedEventArgs e)
        {
            // remember current pos
            var pos = await Editor.GetPositionAsync();

            Editor.Text = "Testing some new content here.\n\tIf you placed your cursor near the start of the text before you hit the button.\nIt should still be in the same spot.";

            await Editor.SetPositionAsync(pos);

            Editor.Focus(FocusState.Programmatic);
        }

        private void ButtonSetSelectedText_Click(object sender, RoutedEventArgs e)
        {
            Editor.SelectedText = "This is some Selected Text!";
        }

        private void ButtonSetReadonly_Click(object sender, RoutedEventArgs e)
        {
            Editor.ReadOnly = !Editor.ReadOnly;
        }

        private async void ButtonRunScript_Click(object sender, RoutedEventArgs e)
        {
            var result = await Editor.ExecuteScriptAsync(@"function test(a, b) { return a + b; }; test(3, 4).toString()");
            Debug.WriteLine(result);
        }
    }
}
