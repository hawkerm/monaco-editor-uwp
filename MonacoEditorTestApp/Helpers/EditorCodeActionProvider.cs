using Monaco;
using Monaco.Editor;
using Monaco.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;

namespace MonacoEditorTestApp.Helpers
{
    public class EditorCodeActionProvider : CodeActionProvider
    {
        private readonly Range _targetRange = new Range(2, 5, 2, 10); // See MarkerData - link:MainPage.xaml.cs:MarkerData

        private string _commandId;

        public bool IsOn { get; set; }

        public EditorCodeActionProvider(string commandId)
        {
            _commandId = commandId;
        }

        public IAsyncOperation<CodeActionList> ProvideCodeActionsAsync(IModel model, Range range, CodeActionContext context)
        {
            /// Will be called whenever the caret moves, you'll get the range update.
            /// Should determine which suggestions to make for that spot.
            /// Use in combination with Markers in the editor to show squiggles on trouble spots.
            return AsyncInfo.Run(async delegate (CancellationToken cancellationToken)
            {
                // In the warning squiggle.
                if (IsOn && _targetRange.ContainsRange(range))
                {
                    return new CodeActionList()
                    {
                        Actions = new CodeAction[] {
                            new CodeAction()
                            {
                                Title = "Suggested Quick Fix",
                                Kind = "quickfix",
                                // If a Command and an Edit are provided the Command happens after the edit, you don't have to provide both though.
                                Command = new Command()
                                {
                                    Id = _commandId,
                                    Title = "Suggestion Command",
                                    Arguments = new object[] { "Suggestion Info" },
                                    Tooltip = "This is a CodeAction Command"
                                },
                                Diagnostics = new IMarkerData[]
                                {
                                    new MarkerData(_targetRange)
                                    {
                                        Code = "2344",
                                        Message = "This is a \"Warning\" about 'that thing'.",
                                        Severity = MarkerSeverity.Warning,
                                        Source = "Origin",
                                    }
                                },
                                // TODO: We should write an extension method helper to create one of these easier maybe?
                                Edit = new WorkspaceEdit()
                                {
                                    Edits = new WorkspaceTextEdit[]
                                    {
                                        new WorkspaceTextEdit()
                                        {
                                            //// Unlike Monaco, we only track one model, so we don't have to pass a resource uri here.
                                            Edit = new TextEdit()
                                            {
                                                Text = "Done",
                                                Range = _targetRange
                                            }
                                        }
                                    }
                                },
                            }
                        }
                    };
                }

                return new CodeActionList();
            });
        }
    }
}
