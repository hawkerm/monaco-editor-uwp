namespace Monaco.Editor
{
    /// <summary>
    /// Selects the folding strategy. 'auto' uses the strategies contributed for the current
    /// document, 'indentation' uses the indentation based folding strategy.
    /// Defaults to 'auto'.
    /// </summary>
    public enum FoldingStrategy { Auto, Indentation };

}
