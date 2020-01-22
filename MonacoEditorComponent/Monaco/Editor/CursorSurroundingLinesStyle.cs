namespace Monaco.Editor
{
    /// <summary>
    /// Controls when `cursorSurroundingLines` should be enforced
    /// Defaults to `default`, `cursorSurroundingLines` is not enforced when cursor position is
    /// changed
    /// by mouse.
    /// </summary>
    public enum CursorSurroundingLinesStyle { All, Default };

}
