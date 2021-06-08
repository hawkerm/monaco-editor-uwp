using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Monaco.Extensions;
using Uno.Foundation;
using Uno.Foundation.Interop;
using Uno.Logging;
using Uno.Extensions;

namespace Monaco
{
    public partial class CodeEditorPresenter : Control, ICodeEditorPresenter, IJSObject
	{
		private static readonly string UNO_BOOTSTRAP_APP_BASE = global::System.Environment.GetEnvironmentVariable(nameof(UNO_BOOTSTRAP_APP_BASE));

		private readonly JSObjectHandle _handle;

		/// <inheritdoc />
		JSObjectHandle IJSObject.Handle => _handle;

		public CodeEditorPresenter()
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
			if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
			{
				this.Log().Debug($"RaiseDOMContentLoaded: Handle is null {_handle == null}");
			}

			if (_handle == null) return;

			if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
			{
				this.Log().Debug($"Raising DOMContentLoaded");
			}

			Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => DOMContentLoaded?.Invoke(null, new WebViewDOMContentLoadedEventArgs()));
		}

		/// <inheritdoc />
		public void AddWebAllowedObject(string name, object pObject)
		{
			if (pObject is IJSObject obj)
			{
				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"AddWebAllowedObject: Add Web Allowed Object - {name}");
				}

				var method = obj.Handle.GetType().GetMethod("GetNativeInstance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"AddWebAllowedObject: Method exists {method != null}");
				}

				var native  = method.Invoke(obj.Handle,new object[] { }) as string;

				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"AddWebAllowedObject: Native handle {native}");
				}

                var htmlId = this.GetHtmlId();

				var script = $@"
					console.log('starting');
					var value = {native};
					console.log('v>' + value);
					var frame = Uno.UI.WindowManager.current.getView({htmlId});
					console.log('f>' + (!frame));
					var frameWindow = window;
					console.log('fw>' + (!frameWindow));

					frameWindow.{name} = value;
					console.log('ended');
					";

				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"AddWebAllowedObject: {script}");
				}

                try
                {
                    WebAssemblyRuntime.InvokeJS(script);
                }
                catch (Exception e)
				{
					if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Error))
					{
						this.Log().Error($"AddWebAllowedObject failed", e);
					}
                }

				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"Add WebAllowed Compeleted");
				}
			}
			else
			{
				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Error))
				{
					this.Log().Error($"AddWebAllowedObject: {name} is not a JSObject");
				}
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
			get => new global::System.Uri(this.GetHtmlAttribute("src"));
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

				if (this.Log().IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					this.Log().Debug($"Loading {target} (Nav is null {NavigationStarting == null})");
				}

				this.SetHtmlAttribute("src", target);

				//NavigationStarting?.Invoke(this, new WebViewNavigationStartingEventArgs());
				Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => NavigationStarting?.Invoke(this, new WebViewNavigationStartingEventArgs()));
			}
		}

		/// <inheritdoc />
		public IAsyncOperation<string> InvokeScriptAsync(string scriptName, IEnumerable<string> arguments)
		{
			var script = $@"(function() {{
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

			if (_log.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
			{
				_log.Debug("Invoke Script: " + script);
			}

			try
			{
				var result = this.ExecuteJavascript(script);

				if (_log.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Debug))
				{
					_log.Debug($"Invoke Script result: {result}");
				}

				return Task.FromResult(result).AsAsyncOperation();
			}
			catch (Exception e)
			{
				if (_log.IsEnabled(Microsoft.Extensions.Logging.LogLevel.Error))
				{
					_log.Error("Invoke Script failed", e);
				}

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
					Debug.log(""Unable to read options - "" + err);
				}}

				Debug.log(""Getting Parent Text value"");
				opt[""value""] = getParentValue(""Text"");

				Debug.log(""Getting Host container"");
				Debug.log(""Creating Editor"");
				const editor = monaco.editor.create(element, opt);
				window.editor= editor;

				Debug.log(""Getting Editor model"");
	            model = editor.getModel();
				window.model = model;

	            // Listen for Content Changes
				Debug.log(""Listening for changes in the editor model - "" + (!model));
	            model.onDidChangeContent((event) => {{
	                   Parent.setValue(""Text"", stringifyForMarshalling(model.getValue()));
	                    //console.log(""buffers: "" + JSON.stringify(model._buffer._pieceTree._buffers));
	                    //console.log(""commandMgr: "" + JSON.stringify(model._commandManager));
	                    //console.log(""viewState:"" + JSON.stringify(editor.saveViewState()));
	                }});

	            // Listen for Selection Changes
				Debug.log(""Listening for changes in the editor selection"");
	            editor.onDidChangeCursorSelection((event) => {{
	                            if (!modifingSelection)
	                            {{
	                                console.log(event.source);
	                        Parent.setValue(""SelectedText"", stringifyForMarshalling(model.getValueInRange(event.selection)));
	                        Parent.setValueWithType(""SelectedRange"", stringifyForMarshalling(JSON.stringify(event.selection)), ""Selection"");
	                        }}
	                    }});

	            // Set theme
				Debug.log(""Getting parent theme value"");
	            let theme = getParentJsonValue(""RequestedTheme"");
	            theme = {{
	                        ""0"": ""Default"",
	                        ""1"": ""Light"",
	                        ""2"": ""Dark""
	                    }}
	                    [theme];
				Debug.log(""Current theme value - "" + theme);
	            if (theme == ""Default"") {{
					Debug.log(""Loading default theme"");

	                theme = getThemeCurrentThemeName();
	            }}
				Debug.log(""Changing theme"");
	            changeTheme(theme, getThemeIsHighContrast());

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

            this.ExecuteJavascript(javascript);

            //Dispatcher.RunAsync(CoreDispatcherPriority.Low, () => NavigationCompleted?.Invoke(this, new WebViewNavigationCompletedEventArgs()));
        }
	}
}