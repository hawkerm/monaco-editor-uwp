Monaco Editor UWP
=================
A *Windows Runtime Component* wrapper around the web-based [Monaco Editor](https://microsoft.github.io/monaco-editor/).  This allows the Monaco Editor to be more easily consumed directly in XAML for C#/C++ UWP based projects.

This project is not affiliated with the Monaco team and is provided for convenience.  Please direct issues related to the use of this control wrapper to this repository.

This control is still in an early alpha state.  Currently, every minor version change will signal breaking changes.

Supported Features
------------------
The following Monaco Editor features are currently supported by this component bridge:

- Two-way Text Binding
- Code Language for Syntax Highlighting
- Stand Alone Code Editor Options
- Decorations
- Actions and Commands
- Basic IModel Support
- KeyDown Events
- Render Aware: Only displays once Loading is complete.

Usage
-----

An x64 NuGet Package is provided.

```
Install-Package Monaco.Editor -Version 0.3.0-alpha
```

Look at the TestApp for current usage.
See [changelog](changelog.md) for more info.

Build Notes
-----------
Built using Visual Studio 2017 for Windows 10 14393 and above.

The **released** complete Monaco v0.9.0 build is used as a reference, this is not included in this repository and can be downloaded from the [Monaco site](https://microsoft.github.io/monaco-editor/).  The contents of its uncompressed 'package' directory should be placed in the *MonacoEditorComponent/monaco-editor* directory.

License
-------
MIT