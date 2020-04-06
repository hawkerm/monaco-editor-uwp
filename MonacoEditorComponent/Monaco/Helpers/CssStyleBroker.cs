using Monaco.Editor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monaco.Helpers
{
    /// <summary>
    /// Broker to help manage CSS Styles and their usage within each CodeEditor. Lifetime managed by CodeEditor.
    /// </summary>
    internal sealed class CssStyleBroker : IDisposable
    {
        private static uint Id = 0;

        // Track styles registered to this particular editor.
        private static readonly Dictionary<uint, WeakReference<ICssStyle>> _registry = new Dictionary<uint, WeakReference<ICssStyle>>();

        private static readonly Dictionary<WeakReference<CodeEditor>, HashSet<uint>> _knownStyles = new Dictionary<WeakReference<CodeEditor>, HashSet<uint>>();

        private static readonly Dictionary<WeakReference<CodeEditor>, bool> _isDirty = new Dictionary<WeakReference<CodeEditor>, bool>();

        private WeakReference<CodeEditor> _parent;

        public CssStyleBroker(CodeEditor codeEditor)
        {
            _parent = new WeakReference<CodeEditor>(codeEditor);
            _knownStyles.Add(_parent, new HashSet<uint>());
            _isDirty.Add(_parent, false);
        }

        public void Dispose()
        {
            _knownStyles.Remove(_parent);
            _isDirty.Remove(_parent);
        }

        /// <summary>
        /// Returns the name for a style to use after registered. Generates a unique style name.
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static uint Register(ICssStyle style)
        {
            var id = Id++;
            _registry.Add(id, new WeakReference<ICssStyle>(style));
            return id;
        }

        public bool AssociateStyles(IModelDeltaDecoration[] decorations)
        {
            /// By construction we assume that decorations will not be null from the call in <see cref="CodeEditor.DeltaDecorationsHelperAsync"/>
            bool newStyle = _isDirty[_parent]; /// Can be set in <see cref="GetStyles"/>.

            _isDirty[_parent] = false; // Reset

            foreach (var decoration in decorations)
            {
                // Add (or ignore) elements to the collection.
                // If any Adds are new, we flag our boolean to return
                if (decoration.Options.ClassName != null)
                {
                    newStyle |= _knownStyles[_parent].Add(decoration.Options.ClassName.Id);
                }
                if (decoration.Options.GlyphMarginClassName != null)
                {
                    newStyle |= _knownStyles[_parent].Add(decoration.Options.GlyphMarginClassName.Id);
                }
                if (decoration.Options.InlineClassName != null)
                {
                    newStyle |= _knownStyles[_parent].Add(decoration.Options.InlineClassName.Id);
                }
            }

            return newStyle;
        }

        /// <summary>
        /// Returns the CSS block for all registered styles.
        /// </summary>
        /// <returns></returns>
        public string GetStyles()
        {
            StringBuilder rules = new StringBuilder(100);
            _knownStyles[_parent].RemoveWhere(id =>
            {
                if (_registry[id].TryGetTarget(out var style))
                {
                    rules.AppendLine(style.ToCss());
                }
                else
                {
                    // Clean-up from disposed style objects
                    foreach (var entry in _knownStyles)
                    {
                        if (entry.Key == _parent)
                        {
                            break; // Skip our current editor, as we can't remove from within the loop. Thus the return true below in this RemoveWhere clause.
                        }
                        entry.Value.Remove(id); // Remove from Style set
                        _isDirty[entry.Key] = true; // Mark that editor as dirty
                    }
                    _registry.Remove(id); // Remove the style completely from our known world as it's gone.

                    return true; // Also remove this entry from our own set.
                }

                return false; // Default, we're just using this as a dumb loop, but that we can remove from when needed above.
            });

            return rules.ToString();
        }
    }
}
