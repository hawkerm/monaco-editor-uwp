using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Uno.Foundation;
using Uno.Foundation.Interop;

namespace Monaco
{
    public partial class CodeEditorPresenter : Control, ICodeEditorPresenter, IJSObject
	{
		private static readonly string UNO_BOOTSTRAP_APP_BASE = global::System.Environment.GetEnvironmentVariable(nameof(UNO_BOOTSTRAP_APP_BASE));

		private readonly JSObjectHandle _handle;

		/// <inheritdoc />
		JSObjectHandle IJSObject.Handle => _handle;

		public CodeEditorPresenter() : base("div")
		{
			//Background = new SolidColorBrush(Colors.Red);
			_handle = JSObjectHandle.Create(this);

			RaiseDOMContentLoaded();


			//WebAssemblyRuntime.InvokeJSWithInterop($@"
			//	console.log(""///////////////////////////////// subscribing to DOMContentLoaded - "" + {HtmlId});

			//	var frame = Uno.UI.WindowManager.current.getView({HtmlId});
				
			//	console.log(""Got view"");

			//	frame.addEventListener(""loadstart"", function(event) {{
			//		var frameDoc = frame.contentDocument;
			//		console.log(""/////////////////////////////////  Frame DOMContentLoaded, subscribing to document"" + frameDoc);
			//		{this}.RaiseDOMContentLoaded();
			//	}}); 
			//	console.log(""Added load start"");



			//	frame.addEventListener(""load"", function(event) {{
			//		var frameDoc = frame.contentDocument;
			//		console.log(""/////////////////////////////////  Frame loaded, subscribing to document"" + frameDoc);
			//		{this}.RaiseDOMContentLoaded();
			//		//frameDoc.addEventListener(""DOMContentLoaded"", function(event) {{
			//		//	console.log(""Raising RaiseDOMContentLoaded"");
			//		//	{this}.RaiseDOMContentLoaded();
			//		//}});
			//	}}); 

			//	console.log(""Added load"");


			//	");
		}

		public void RaiseDOMContentLoaded()
		{
			Console.Error.WriteLine($"Handle is null {_handle == null}");
			if (_handle == null) return;

			Console.Error.WriteLine("-------------------------------------------------------- RaiseDOMContentLoaded");
			Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => DOMContentLoaded?.Invoke(null, new WebViewDOMContentLoadedEventArgs()));
		}

		/// <inheritdoc />
		protected override void OnLoaded()
		{
			base.OnLoaded();

			/*Console.Error.WriteLine("---------------------- LOADED ");


			var script = $@"
					var frame = Uno.UI.WindowManager.current.getView({HtmlId});
					var frameDoc = frame.contentDocument;
					
					return frameDoc.onload = function() { };

			Console.Error.WriteLine("***************************************** AddWebAllowedObject: " + script);*/
		}

		/// <inheritdoc />
		public void AddWebAllowedObject(string name, object pObject)
		{
			if (pObject is IJSObject obj)
			{
				Console.Error.WriteLine($"Add Web Allowed Object - {name}");
				var method = obj.Handle.GetType().GetMethod("GetNativeInstance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
				Console.Error.WriteLine($"*** Method exists {method != null}");
				var native  = method.Invoke(obj.Handle,new object[] { }) as string;
				Console.Error.WriteLine($"*** Native handle {native}");


				var script = $@"
					console.log('starting');
					var value = {native};
					console.log('v>' + value);
					var frame = Uno.UI.WindowManager.current.getView({HtmlId});
					console.log('f>' + (!frame));
					var frameWindow = window;
					console.log('fw>' + (!frameWindow));

					frameWindow.{name} = value;
					console.log('ended');
					";
                ////frameWindow.eval(""var {name} = window.parent.{obj.Handle.GetNativeInstance().Replace("\"", "\\\"")}; ""); 

                Console.Error.WriteLine("***************************************** AddWebAllowedObject: " + script);

                try
                {
                    WebAssemblyRuntime.InvokeJS(script);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("FAILED " + e);
                }

				Console.Error.WriteLine("Add WebAllowed Compeleted");
			}
			else
			{
				Console.Error.WriteLine(name + " is not a JSObject :( :( :( :( :( :( :( :( :( :( :( :( :( :( :( :( :( :( :( :( ");
			}
		}

		/// <inheritdoc />
		public event TypedEventHandler<ICodeEditorPresenter, WebViewNewWindowRequestedEventArgs> NewWindowRequested; // ignored for now (external navigation)

		/// <inheritdoc />
		public event TypedEventHandler<ICodeEditorPresenter, WebViewNavigationStartingEventArgs> NavigationStarting;

		/// <inheritdoc />
		public event TypedEventHandler<ICodeEditorPresenter, WebViewDOMContentLoadedEventArgs> DOMContentLoaded;

		/// <inheritdoc />
		public event TypedEventHandler<ICodeEditorPresenter, WebViewNavigationCompletedEventArgs> NavigationCompleted; // ignored for now (only focus the editor)

		/// <inheritdoc />
		public global::System.Uri Source
		{
			get => new global::System.Uri(GetAttribute("src"));
			set
			{
                //var path = Environment.GetEnvironmentVariable("UNO_BOOTSTRAP_APP_BASE");
                //var target = $"/{path}/MonacoCodeEditor.html";
                //var target = (value.IsAbsoluteUri && value.IsFile)
                //	? value.PathAndQuery 
                //	: value.ToString();

                string target;
				if (value.IsAbsoluteUri)
				{
					if(value.Scheme=="file")
					{
						// Local files are assumed as coming from the remoter server
						target = UNO_BOOTSTRAP_APP_BASE == null ? value.PathAndQuery : UNO_BOOTSTRAP_APP_BASE + value.PathAndQuery;
					}
                    else
                    {
						target = value.AbsoluteUri;

					}

				}
				else
				{
					target = UNO_BOOTSTRAP_APP_BASE == null
						? value.OriginalString
						: UNO_BOOTSTRAP_APP_BASE + "/" + value.OriginalString;
				}

				Console.Error.WriteLine("***** LOADING: " + target);

				Console.Error.WriteLine($"---- Nav is null {NavigationStarting == null}");

				SetAttribute("src", target);

				//NavigationStarting?.Invoke(this, new WebViewNavigationStartingEventArgs());
				Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => NavigationStarting?.Invoke(this, new WebViewNavigationStartingEventArgs()));

			}
		}

		/// <inheritdoc />
		public IAsyncOperation<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
		{
			Console.WriteLine("+++++++++++++++++++++++++++++++ Invoke Script +++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			var script = $@"(function() {{
				//var frame = Uno.UI.WindowManager.current.getView({HtmlId});
				//var frameWindow = frame.contentWindow;
				
				try {{
					window.__evalMethod = function() {{ {arguments.Single()} }};
					
					return window.eval(""__evalMethod()"") || """";
				}}
				catch(err){{
					Debug.log(err);
				}}
				finally {{
					window.__evalMethod = null;
				}}
			}})()";
			Console.Error.WriteLine(script);

			try
			{
				var result = WebAssemblyRuntime.InvokeJS(script);

				Console.WriteLine("Ok++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
				Console.WriteLine(result);

				return Task.FromResult(result).AsAsyncOperation();
			}
			catch (Exception e)
			{
				Console.WriteLine("ERR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
				Console.WriteLine(e);

				return Task.FromResult("").AsAsyncOperation();
			}
		}

		public void Launch()
		{
			string javascript = $@"
        (function(){{

			Debug.log(""Create dynamic style element"");
			var head = document.head || document.getElementsByTagName('head')[0];
			var style = document.createElement('style');
			style.id='dynamic';
			head.appendChild(style);

            Debug.log(""Starting Monaco Load"");

            var editor;
            var model;
            var contexts = {{}};
			window.contexts={{}};
            var decorations = [];
			window.decorations=[];
            var modifingSelection = false; // Supress updates to selection when making edits.
			window.modifyingSelection=false;

			require.config({{ paths: {{ 'vs': '{UNO_BOOTSTRAP_APP_BASE}/monaco-editor/min/vs' }} }});
			require(['vs/editor/editor.main'], function () {{


				Debug.log(""Grabbing Monaco Options"");

				var opt = {{}};
				try{{
					opt = getOptions();
				}}
				catch(err){{
					Debug.log(""Unable to read options"");
				}}

				Debug.log(""Getting Parent Text value"");
				opt[""value""] = Parent.getValue(""Text"");

				Debug.log(""Getting Host container"");
				var container= Uno.UI.WindowManager.current.getView({HtmlId});
				var containerElement = document.getElementById({HtmlId});
				Debug.log(""Creating Editor"");
				editor = monaco.editor.create(containerElement, opt);
				window.editor= editor;

				Debug.log(""Getting Editor model"");
	            model = editor.getModel();
				window.model = model;

	   //         // Listen for Content Changes
				//Debug.log(""Listening for changes in the editor model - "" + (!model));
	   //         model.onDidChangeContent((event) => {{
	   //                Parent.setValue(""Text"", model.getValue());
	   //                 //console.log(""buffers: "" + JSON.stringify(model._buffer._pieceTree._buffers));
	   //                 //console.log(""commandMgr: "" + JSON.stringify(model._commandManager));
	   //                 //console.log(""viewState:"" + JSON.stringify(editor.saveViewState()));
	   //             }});

	   //         // Listen for Selection Changes
				//Debug.log(""Listening for changes in the editor selection"");
	   //         editor.onDidChangeCursorSelection((event) => {{
	   //                         if (!modifingSelection)
	   //                         {{
	   //                             console.log(event.source);
	   //                     Parent.setValue(""SelectedText"", model.getValueInRange(event.selection));
	   //                     Parent.setValue(""SelectedRange"", JSON.stringify(event.selection), ""Selection"");
	   //                     }}
	   //                 }});

	            // Set theme
				Debug.log(""Getting parent theme value"");
	            let theme = Parent.getJsonValue(""RequestedTheme"");
	            theme = {{
	                        ""0"": ""Default"",
	                        ""1"": ""Light"",
	                        ""2"": ""Dark""
	                    }}
	                    [theme];
				Debug.log(""Current theme value - "" + theme);
	            if (theme == ""Default"") {{
					Debug.log(""Loading default theme"");

	                theme = Theme.currentThemeName.toString();
	            }}
				Debug.log(""Changing theme"");
	            changeTheme(theme, 'false');//Theme.isHighContrast.toString());

	            // Update Monaco Size when we receive a window resize event
				Debug.log(""Listen for resize events on the window and resize the editor"");
	            window.addEventListener(""resize"", () => {{
	                            editor.layout();
	                        }});

	            // Disable WebView Scrollbar so Monaco Scrollbar can do heavy lifting
	            document.body.style.overflow = 'hidden';

	            // Callback to Parent that we're loaded
	            Debug.log(""Loaded Monaco"");
	            Parent.callAction(""Loaded"");

	            Debug.log(""Ending Monaco Load"");
			}});
        }})();";
			WebAssemblyRuntime.InvokeJS(javascript);

			Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => NavigationCompleted?.Invoke(this, new WebViewNavigationCompletedEventArgs()));
		}
	}
}