namespace Monaco.Helpers
{
    public sealed class CssGlyphStyle : ICssStyle
    {
        public System.Uri GlyphImage { get; set; }

        public uint Id { get; }

        public string Name { get; }

        public CssGlyphStyle()
        {
            Id = CssStyleBroker.Register(this);
            Name = "generated-style-" + Id;
        }

        public string ToCss()
        {
            return this.WrapCssClassName(string.Format("background: url(\"{0}\");", GlyphImage.AbsoluteUri));
        }
    }
}
