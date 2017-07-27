using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.Helpers
{
    public interface ICssStyle
    {
        string Name { get; }

        string ToCss();
    }

    /// <summary>
    /// Singleton Broker to help us manage CSS Styles
    /// </summary>
    public sealed class CssStyleBroker
    {
        private static uint Id = 0;
        private Dictionary<string, ICssStyle> _registered = new Dictionary<string, ICssStyle>();

        // http://csharpindepth.com/Articles/General/Singleton.aspx
        private static readonly CssStyleBroker _instance = new CssStyleBroker();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CssStyleBroker()
        {
        }
        private CssStyleBroker()
        {
        }
        public static CssStyleBroker Instance // TODO: Probably need to tie this to a specific Editor
        {
            get
            {
                return _instance;
            }
        }
        
        /// <summary>
        /// Returns the name for a style to use after registered.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public string Register(ICssStyle style)
        {
            CssStyleBroker.Id += 1;
            var name = "generated-style-" + Id;
            this._registered.Add(name, style);
            return name;
        }

        /// <summary>
        /// Returns the CSS block for all registered styles.
        /// </summary>
        /// <returns></returns>
        public string GetStyles()
        {
            StringBuilder rules = new StringBuilder(100);
            foreach (ICssStyle css in _registered.Values)
            {
                rules.AppendLine(css.ToCss());
            }
            return rules.ToString();
        }

        public static string WrapCssClassName(ICssStyle style, string inner)
        {
            return String.Format(".{0} {{ {1} }}", style.Name, inner);
        }
    }
}
