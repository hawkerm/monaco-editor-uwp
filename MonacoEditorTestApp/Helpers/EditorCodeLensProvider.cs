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
    public class EditorCodeLensProvider : CodeLensProvider
    {
        private string _commandId;

        public EditorCodeLensProvider(string commandId)
        {
            _commandId = commandId;
        }

        public IAsyncOperation<CodeLensList> ProvideCodeLensesAsync(IModel model)
        {
            return AsyncInfo.Run(async delegate (CancellationToken cancellationToken)
            {
                return new CodeLensList()
                {
                    Lenses = new CodeLens[] {
                        new CodeLens()
                        {
                            Id = "Third Line",
                            Range = new Range(4, 1, 5, 1),
                            Command = new Command()
                            {
                                Id = _commandId,
                                Title = "Third Line Command",
                                Arguments = new object[] { "First Argument" , 5, new float[] { 4.5f, 0.3f } }, // Note: This 3rd element array will come back as a JArray TODO?
                                Tooltip = "This is a CodeLens Command"
                            }
                        }
                    }
                };
            });
        }

        public IAsyncOperation<CodeLens> ResolveCodeLensAsync(IModel model, CodeLens codeLens)
        {
            return AsyncInfo.Run(delegate (CancellationToken cancelationToken) {
                return Task.FromResult(codeLens);
            });                
        }
    }
}
