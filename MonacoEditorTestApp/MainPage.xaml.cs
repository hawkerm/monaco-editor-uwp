using Monaco;
using Monaco.Editor;
using Monaco.Helpers;
using MonacoEditorTestApp.Actions;
using MonacoEditorTestApp.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Text;
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

        private EditorCodeActionProvider _actionProvider;

        #region CSS Style Objects
        private readonly CssLineStyle CssLineDarkTransparentRed = new CssLineStyle()
        {
            BackgroundColor = Color.FromArgb(128, 128, 0, 0),
        };

        private readonly CssLineStyle CssLineLightBlue = new CssLineStyle()
        {
            BackgroundColor = Colors.LightBlue
        };

        private readonly CssInlineStyle CssInlineWhiteBold = new CssInlineStyle()
        {
            ForegroundColor = Colors.White,
            FontWeight = FontWeights.Bold,
            FontStyle = FontStyle.Italic
        };

        private readonly CssInlineStyle CssInlineStrikeThrough = new CssInlineStyle()
        {
            TextDecoration = TextDecoration.LineThrough
        };

        private readonly CssGlyphStyle CssGlyphError = new CssGlyphStyle()
        {
            GlyphImage = new System.Uri("ms-appx-web:///Icons/error.png")
        };

        private readonly CssGlyphStyle CssGlyphWarning = new CssGlyphStyle()
        {
            GlyphImage = new System.Uri("ms-appx-web:///Icons/warning.png")
        };
        #endregion

        public MainPage()
        {
            InitializeComponent();
            options = Editor.Options;
            Editor.Loading += Editor_Loading;
            Editor.Loaded += Editor_Loaded;
            Editor.OpenLinkRequested += Editor_OpenLinkRequest;

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

                // Set the copy of our initial text.
                TextEditor.Text = CodeContent;

                ButtonHighlightRange_Click(null, null);
            }

            // Ready for Code

            var available_languages = Editor.Languages.GetLanguagesAsync();
            //Debugger.Break();

            // Code Action Command - TODO: Should we just encapsulate these in the Provider class anyway as they're only being used there?
            string cmdId = await Editor.AddCommandAsync(async (args) =>
            {
                var md = new MessageDialog($"You hit the CodeAction command, Arg[0] = {args[0]}");
                await md.ShowAsync();
            });

            // Code Lens Command
            string cmdId2 = await Editor.AddCommandAsync(async (args) =>
            {
                var md = new MessageDialog($"You hit the CodeLens command, Arg[0] = {args[0]}, Arg[1] = {args[1]}, Args[2] = {args[2]}");
                await md.ShowAsync();
            });

            _actionProvider = new EditorCodeActionProvider(cmdId);

            await Editor.Languages.RegisterCodeActionProviderAsync("csharp", _actionProvider);

            await Editor.Languages.RegisterCodeLensProviderAsync("csharp", new EditorCodeLensProvider(cmdId2));

            await Editor.Languages.RegisterColorProviderAsync("csharp", new ColorProvider());

            await Editor.Languages.RegisterCompletionItemProviderAsync("csharp", new LanguageProvider());

            await Editor.Languages.RegisterHoverProviderAsync("csharp", new EditorHoverProvider());

            _myCondition = await Editor.CreateContextKeyAsync("MyCondition", false);

            await Editor.AddCommandAsync(KeyCode.F5, async (args) => {
                var md = new MessageDialog("You Hit F5!");
                await md.ShowAsync();

                // Turn off Command again.
                _myCondition?.Reset();

                // Refocus on CodeEditor
                Editor.Focus(FocusState.Programmatic);
            }, _myCondition.Key);

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_R, async (args) =>
            {
                var range = await Editor.GetModel().GetFullModelRangeAsync();

                var md = new MessageDialog("Document Range: " + range.ToString());
                await md.ShowAsync();

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_W, async (args) =>
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

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_L, async (args) =>
            {
                var model = Editor.GetModel();
                var line = await model.GetLineContentAsync((await Editor.GetPositionAsync()).LineNumber);
                var lines = await model.GetLinesContentAsync();
                var count = await model.GetLineCountAsync();

                var md = new MessageDialog("Current Line: " + line + "\nAll Lines [" + count + "]:\n" + string.Join("\n", lines));
                await md.ShowAsync();

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddCommandAsync(KeyMod.CtrlCmd | KeyCode.KEY_U, async (args) =>
            {
                var range = new Range(2, 10, 3, 8);
                var seg = await Editor.GetModel().GetValueInRangeAsync(range);

                var md = new MessageDialog("Segment " + range.ToString() + ": " + seg);
                await md.ShowAsync();

                Editor.Focus(FocusState.Programmatic);
            });

            await Editor.AddActionAsync(new TestAction());

            // we only need to fire loading once to initialize all our model/helpers
            Editor.Loading -= Editor_Loading;
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            // Ready for Display
            Editor.Loaded -= Editor_Loaded;
        }

        private void Editor_OpenLinkRequest(WebView sender, WebViewNewWindowRequestedEventArgs args)
        {
            if (this.AllowWeb.IsChecked == false)
            {
                args.Handled = true;
            }
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
            Editor.Decorations.Add(
                new IModelDeltaDecoration(new Range(3, 1, 3, 10), new IModelDecorationOptions()
                {
                    ClassName = CssLineDarkTransparentRed,
                    InlineClassName = CssInlineWhiteBold,
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
                    ClassName = CssLineLightBlue,
                    InlineClassName = CssInlineWhiteBold,
                    GlyphMarginClassName = CssGlyphError,
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
                    InlineClassName = CssInlineStrikeThrough,
                    GlyphMarginClassName = CssGlyphWarning,
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
        private void Editor_KeyDown(CodeEditor sender, WebKeyEventArgs e)
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
                _ = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                {
                    var md = new MessageDialog("You Hit Ctrl+Enter!");
                    await md.ShowAsync();

                    // Refocus on CodeEditor
                    Editor.Focus(FocusState.Programmatic);
                });

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

        private void ButtonLineNumbers_Click(object sender, RoutedEventArgs e)
        {
            options.LineNumbers = options.LineNumbers switch
            {
                LineNumbersType.Interval => LineNumbersType.Off,
                LineNumbersType.Off => LineNumbersType.On,
                LineNumbersType.On or null => LineNumbersType.Relative,
                LineNumbersType.Relative => LineNumbersType.Interval,
                _ => throw new NotImplementedException(),
            };
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
                        StartColumn = 5,
                        EndLineNumber = 2,
                        EndColumn = 10
                    });

                Editor.Markers.Add(
                    new MarkerData()
                    {
                        Code = "2345",
                        Message = "This is an \"Error\" about 'that thing'.",
                        Severity = MarkerSeverity.Error,
                        Source = "Origin",
                        StartLineNumber = 5,
                        StartColumn = 3,
                        EndLineNumber = 5,
                        EndColumn = 10
                    });

                _actionProvider.IsOn = true;
            }
            else
            {
                Editor.Markers.Clear();

                _actionProvider.IsOn = false;
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

                Editor.OpenLinkRequested -= Editor_OpenLinkRequest;
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

                // Re-setup loading events for new instance
                Editor.Loading += Editor_Loading;
                Editor.Loaded += Editor_Loaded;
                Editor.OpenLinkRequested += Editor_OpenLinkRequest;
                Editor.InternalException += Editor_InternalException;

                Grid.SetColumn(Editor, 1);

                RootGrid.Children.Add(Editor);

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
            var result = await Editor.InvokeScriptAsync(@"function test(a, b) { return a + b; }; test(3, 4).toString()");

            var md = new MessageDialog($"Result (should be 7): {result}");
            await md.ShowAsync();
        }

        private void Editor_GotFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Editor Got Focus");
        }

        private void Editor_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Editor Lost Focus");
        }

        private async void ButtonFindCtrl_Click(object sender, RoutedEventArgs e)
        {
            var matches = await Editor.GetModel().FindMatchesAsync("Ctrl", true, false, true, null, true);

            var toString = matches.Select((match) => $"Range: {match.Range.ToString()} contains {match.Matches.Length} Matches - First is: {match.Matches[0]}");

            var output = string.Join("\n", toString);

            var md = new MessageDialog("Results:\n" + output);
            await md.ShowAsync();
        }
    }
}
