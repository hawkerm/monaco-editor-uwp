using Monaco;
using Monaco.Editor;
using Monaco.Languages;
using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonacoEditorTestApp
{
    class EditorHoverProvider : HoverProvider
    {
        public IAsyncOperation<Hover> ProvideHover(IModel model, Position position)
        {
            return AsyncInfo.Run(async delegate (CancellationToken cancelationToken)
            {
                var word = await model.GetWordAtPositionAsync(position);
                if (word != null && word.Word != null && word.Word.IndexOf("Hit", 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    return new Hover(new string[]
                    {
                        "*Hit* - press the keys following together.",
                        "Some **more** text is here.",
                        "And a [link](https://www.github.com/)."
                    }, new Range(position.LineNumber, position.Column, position.LineNumber, position.Column + 5));
                }

                return default(Hover);
            });
        }
    }
}
