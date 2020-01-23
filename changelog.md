v0.9 - 01/23/2020
-----------------
- Updated Monaco Reference to v0.19.3
- Fix broken snippet function
- Tie css styles to CodeEditor

**Breaking changes**: 
- Use StandaloneEditorConstructionOptions for Options instead of IEditorConstructionOptions
- Use ClassName string instead of CssStyle type

v0.8.1 - 01/15/2019
-------------------
- Revert min target back to 14393
- Change Dependencies back

v0.8 - 01/15/2019
-----------------
- Updated min target to 16299.
- Add more inline decoration style properties.
- Add SetPositionAsync. Fixes #24.
- Update NuGet dependencies
- Work for .NET Native
- Changed returns from IWordAtPosition to WordAtPosition for now for .NET Native.

v0.7 - 07/22/2018 
----------------- 
- **Breaking:** `IModelDecorationOptions` now uses `IMarkdownString` for both `HoverMessage` and `GlyphMarginHoverMessage` to reflect change in Monaco API.  A convenience `string` and `string[]` extension `ToMarkdownString` has been provided.  Fixes #22. 
- Updated Monaco Reference to v0.13.0 
- Added initial Language Provider APIs 
  - CompletionItem (IntelliSense, Snippets, etc...) 
  - Hover
- Added `install-dependencies.ps1` script to pull down required Monaco reference. 
- Test app loads content from file now and provides info on things to try. 
 
v0.6 - 05/04/2018
-----------------
- Updated Monaco Reference to v0.12.0
- Added Two-way Binding for `SelectedText` and `SelectedRange` property.
- Fixed Theme Listening to Respect `RequestedTheme` (will not listen to parent changes, but if set will update.) #9
- Cleaned-up Memory Usage from control disposal. #18

v0.5 - 11/14/2017
-----------------
- **Breaking:** Appended 'Async' to Get/SetModelMarkers methods.
- Error Handling with *InternalException* event for all calls.
- Fixes #16

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
 