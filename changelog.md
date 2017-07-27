v0.2 - 07/27/2017
-----------------
- Breaking: Renamed 'Editor' to 'CodeEditor' for component.
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
 