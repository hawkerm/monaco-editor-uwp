using Monaco.Editor;
using System;
using System.ComponentModel;
using Windows.Foundation;

namespace Monaco.Languages
{
    public interface CodeLensProvider
    {
        IAsyncOperation<CodeLensList> ProvideCodeLensesAsync(IModel model);

        IAsyncOperation<CodeLens> ResolveCodeLensAsync(IModel model, CodeLens codeLens);
    }
}

