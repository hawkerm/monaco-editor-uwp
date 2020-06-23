using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace Monaco
{
    public sealed partial class CodeEditorPresenter : UserControl, ICodeEditorPresenter
	{
		private readonly WebView internalWebView;
		public CodeEditorPresenter()
        {
			Content = internalWebView = new WebView();

			internalWebView.NewWindowRequested += (wv, args) => NewWindowRequested(this, args);
			internalWebView.NavigationStarting += (wv, args) => NavigationStarting(this, args);
			internalWebView.DOMContentLoaded += (wv, args) => DOMContentLoaded(this, args);
			internalWebView.NavigationCompleted += (wv, args) => NavigationCompleted(this, args);
		}

		public System.Uri Source { get => internalWebView.Source; set => internalWebView.Source = value; }

        public event TypedEventHandler<ICodeEditorPresenter, WebViewNewWindowRequestedEventArgs> NewWindowRequested;
        public event TypedEventHandler<ICodeEditorPresenter, WebViewNavigationStartingEventArgs> NavigationStarting;
        public event TypedEventHandler<ICodeEditorPresenter, WebViewDOMContentLoadedEventArgs> DOMContentLoaded;
        public event TypedEventHandler<ICodeEditorPresenter, WebViewNavigationCompletedEventArgs> NavigationCompleted;

        public void AddWebAllowedObject(string name, object pObject)
        {
			internalWebView.AddWebAllowedObject(name, pObject);
        }

        public IAsyncOperation<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
        {
			return internalWebView.InvokeScriptAsync(scriptName, arguments);

		}
    }
}