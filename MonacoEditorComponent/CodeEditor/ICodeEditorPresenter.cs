using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Monaco
{
    public interface ICodeEditorPresenter
	{
		/// <summary>Adds a native Windows Runtime object as a global parameter to the top level document inside of a WebView.</summary>
		/// <param name="name">The name of the object to expose to the document in the WebView.</param>
		/// <param name="pObject">The object to expose to the document in the WebView.</param>
		void AddWebAllowedObject(string name, object pObject);

		// <summary>Occurs when a user performs an action in a WebView that causes content to be opened in a new window.</summary>
		event TypedEventHandler<ICodeEditorPresenter, WebViewNewWindowRequestedEventArgs> NewWindowRequested;

		/// <summary>Occurs before the WebView navigates to new content.</summary>
		event TypedEventHandler<ICodeEditorPresenter, WebViewNavigationStartingEventArgs> NavigationStarting;

		/// <summary>Occurs when the WebView has finished parsing the current HTML content.</summary>
		event TypedEventHandler<ICodeEditorPresenter, WebViewDOMContentLoadedEventArgs> DOMContentLoaded;

		/// <summary>Occurs when the WebView has finished loading the current content or if navigation has failed.</summary>
		event TypedEventHandler<ICodeEditorPresenter, WebViewNavigationCompletedEventArgs> NavigationCompleted;

		/// <summary>Gets or sets the Uniform Resource Identifier (URI) source of the HTML content to display in the WebView control.</summary>
		/// <returns>The Uniform Resource Identifier (URI) source of the HTML content to display in the WebView control.</returns>
		global::System.Uri Source { get; set; }

		CoreDispatcher Dispatcher { get; }

		IAsyncOperation<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments);

		bool Focus(FocusState state);
	}
}