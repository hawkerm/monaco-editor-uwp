Monaco Editor UWP
=================
A *Windows Runtime Component* wrapper around the web-based [Monaco Editor](https://microsoft.github.io/monaco-editor/).  This allows the Monaco Editor to be more easily consumed directly in XAML for C#/C++ UWP based projects.

This project is not affiliated with the Monaco team and is provided for convenience.  Please direct issues related to the use of this control wrapper to this repository.

This control is still in an early alpha state.

Usage
-----

An x64 NuGet Package is provided.

```
Install-Package Monaco.Editor
```

Look at the TestApp for current usage.

Current Features
----------------
 - Two-way Text Binding for Code Content Setting and Retrieval
 - CodeLanguage property to set initial syntax highlighting (must be set in XAML declaration).
 - Support for *await new Monaco.LanguagesHelper(Editor).GetLanguagesAsync()* call to retrieve supported languages, use **Id** field in property above.
 - Theme Aware: Control automatically picks theme based on system/app light/dark theme and high contrast settings.

Build Notes
-----------
Built using Visual Studio 2017 for Windows 10 14393 and above.

The **released** complete Monaco build is used as a reference, this is not included in this repository and can be downloaded from the [Monaco site](https://microsoft.github.io/monaco-editor/).  The contents of its uncompressed 'package' directory should be placed in the *MonacoEditorComponent/monaco-editor* directory.

License
-------
MIT