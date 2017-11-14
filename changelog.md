v0.4 - 11/13/2017
-----------------
- Added support for *[Markers](https://microsoft.github.io/monaco-editor/api/modules/monaco.editor.html#setmodelmarkers)*
- Added Opening/Intercepting URIs typed in the Editor.
- Added *InternalException* event for better pre-release error information.
- Fixes for #5, #7, #8, #10, #12
- Cleaned-up Sample App Presentation

v0.3 - 08/21/2017
-----------------
- **Breaking:** Removed access to *DeltaDecorationsAsync*, use *CodeEditor.Decorations* collection instead.  This simplifies usage of the control in the asynchronous environment.
- Added support for *[Action](https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.istandalonecodeeditor.html#addaction)* and *[Command](https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.istandalonecodeeditor.html#addcommand)* editor extensions.
- Added *[IEditorOptions](https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.ieditoroptions.html)* support through the *CodeEditor.Options* property, primary-level property auto-update is supported.
- Added initial single *[IModel](https://microsoft.github.io/monaco-editor/api/interfaces/monaco.editor.imodel.html)* support through *CodeEditor.GetModel()*.
- Added support to retrieve **SelectedText**.
- Use minified Monaco library and build for Any CPU.

v0.2 - 07/27/2017
-----------------
- **Breaking:** Renamed 'Editor' to 'CodeEditor' for component.
- Added basic line highlighting support with *DeltaDecorationsAsync*
- Added preliminary KeyDown event support.
- Added Loading/Loaded event distinction.

v0.1 - 07/24/2017
-----------------
 - Two-way text binding for code content setting and retrieval.
 - CodeLanguage property to set initial syntax highlighting (must be set in XAML declaration).
 - Support for *await new Monaco.LanguagesHelper(Editor).GetLanguagesAsync()* call to retrieve supported languages, use **Id** field in property above.
 - Theme Aware: Control automatically picks theme based on system/app light/dark theme and high contrast settings.
 - Render Aware: Control only displays once Code Editor has been loaded.
 