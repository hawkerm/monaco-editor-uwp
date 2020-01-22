namespace Monaco.Editor
{
    /// <summary>
    /// Control indentation of wrapped lines. Can be: 'none', 'same', 'indent' or 'deepIndent'.
    /// Defaults to 'same' in vscode and to 'none' in monaco-editor.
    /// </summary>
    public enum WrappingIndent { DeepIndent, Indent, None, Same };

}
