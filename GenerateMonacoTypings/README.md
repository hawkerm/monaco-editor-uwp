Generate Monaco Typings
=======================

This [Node.js](https://nodejs.org/) project is provided as a simple way to download the required dependencies needed to take the Monaco TypeScript definition file and generate C# classes for the API via the [TypedocConverter](https://github.com/hez2010/TypedocConverter) project. 

This document assumes you have a Node.js environment setup running on Windows (as PowerShell is also required), but all other dependencies should be installed/setup as part of the script.

To get started, in this directory just run:

```
npm install
```

Voila! C# Typings should be generated! Run `npm install` or `npm run postinstall` to re-generate typings again.

**Note:** The script is configured to overwrite the existing definitions within the main repo, you can configure an alternate output directory (`outdir`) via the `npm config set` command. It defaults to the `MonacoEditorComponent` directory and the namespace will automatically create a `Monaco` sub-directory.

**Note:** This script is currently meant as a guide-post, the typings generated are not all meant to be consumed directly by the project, and certain interfaces have been sculpted to provide a better experience to C# developers. This tool is mostly meant to boot-strap enabling features from the Monaco API and adapting to new versions of the Monaco API. If you have any questions or need a specific feature, please first file an issue on [our repo](https://github.com/hawkerm/monaco-editor-uwp). Thanks!
