using Monaco.Editor;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace Monaco
{
    partial class CodeEditor
    {
        /// <summary>
        /// Get or Set the CodeEditor Text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty TextPropertyField =
            DependencyProperty.Register("Text", typeof(string), typeof(CodeEditor), new PropertyMetadata("", (d, e) => {
                //(d as Canvas)?.InvokeScriptAsync("updateToolbox", new string[] { e.NewValue.ToString() });
                //(d as CodeEditor).CodeChanged?.Invoke(d, e);

                (d as CodeEditor)?.InvokeScriptAsync("updateContent", e.NewValue.ToString());
            }));

        public static DependencyProperty TextProperty
        {
            get
            {
                return TextPropertyField;
            }
        }

        /// <summary>
        /// Get the current Primary Selected CodeEditor Text.
        /// </summary>
        public string SelectedText
        {
            get { return (string)GetValue(SelectedTextProperty); }
            private set { SetValue(SelectedTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty SelectedTextPropertyField =
            DependencyProperty.Register("SelectedText", typeof(string), typeof(CodeEditor), new PropertyMetadata(string.Empty));

        public static DependencyProperty SelectedTextProperty
        {
            get
            {
                return SelectedTextPropertyField;
            }
        }

        /// <summary>
        /// Set the Syntax Language for the Code CodeEditor.
        /// 
        /// Note: Most likely to change or move location.
        /// </summary>
        public string CodeLanguage
        {
            get { return (string)GetValue(CodeLanguageProperty); }
            set { SetValue(CodeLanguageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty CodeLanguagePropertyField =
            DependencyProperty.Register("CodeLanguage", typeof(string), typeof(CodeEditor), new PropertyMetadata("csharp", (d, e) => {
                // https://microsoft.github.io/monaco-editor/api/modules/monaco.editor.html#setmodellanguage.
                (d as CodeEditor)?.InvokeScriptAsync("updateLanguage", e.NewValue.ToString());
            }));

        internal static DependencyProperty CodeLanguageProperty
        {
            get
            {
                return CodeLanguagePropertyField;
            }
        }

        /// <summary>
        /// Get or set the CodeEditor Options.
        /// </summary>
        public IEditorConstructionOptions Options
        {
            get { return (IEditorConstructionOptions)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Options.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty OptionsPropertyField =
            DependencyProperty.Register("Options", typeof(IEditorConstructionOptions), typeof(CodeEditor), new PropertyMetadata(new IEditorConstructionOptions(), (d, e) => {
                var value = e.NewValue as IEditorConstructionOptions;
                var editor = d as CodeEditor;
                editor?.InvokeScriptAsync("updateOptions", value.ToJson());

                // Register for sub-property changes on new object
                // TODO: Need to do this for initial object :(
                if (value != null)
                {
                    value.PropertyChanged += async (s, p) =>
                    {
                        await editor?.InvokeScriptAsync("updateOptions", (s as IEditorConstructionOptions)?.ToJson());
                    };
                }
            }));

        public static DependencyProperty OptionsProperty
        {
            get
            {
                return OptionsPropertyField;
            }
        }

        /// <summary>
        /// Get or Set the CodeEditor Text.
        /// </summary>
        public bool HasGlyphMargin
        {
            get { return (bool)GetValue(HasGlyphMarginProperty); }
            set { SetValue(HasGlyphMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty HasGlyphMarginPropertyField =
            DependencyProperty.Register("HasGlyphMargin", typeof(bool), typeof(CodeEditor), new PropertyMetadata(false, (d, e) => {
                (d as CodeEditor).Options.GlyphMargin = e.NewValue as bool?;
            }));

        public static DependencyProperty HasGlyphMarginProperty
        {
            get
            {
                return HasGlyphMarginPropertyField;
            }
        }

        /// <summary>
        /// Get or set the Line Decorations.
        /// </summary>
        public IObservableVector<IModelDeltaDecoration> Decorations
        {
            get { return (IObservableVector<IModelDeltaDecoration>)GetValue(DecorationsProperty); }
            set { SetValue(DecorationsProperty, value); }
        }

        private AsyncLock _mutexLineDecorations = new AsyncLock();
        private string[] _decorations = Array.Empty<string>();

        // Using a DependencyProperty as the backing store for Options.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty DecorationsPropertyField =
            DependencyProperty.Register("Decorations", typeof(IModelDeltaDecoration), typeof(CodeEditor), new PropertyMetadata(null, async (d, e) => {
                var editor = d as CodeEditor;
                if (editor != null)
                {
                    // We only want to do this one at a time per editor.
                    using (await editor._mutexLineDecorations.LockAsync())
                    {
                        var old = e.OldValue as IObservableVector<IModelDeltaDecoration>;
                        // Clear out the old line decorations if we're replacing them or setting back to null
                        if ((old != null && old.Count > 0) ||
                             e.NewValue == null)
                        {
                            editor._decorations = (await editor.DeltaDecorationsAsync(editor._decorations, null)).ToArray();
                        }
                        var value = e.NewValue as IObservableVector<IModelDeltaDecoration>;

                        if (value != null)
                        {
                            if (value.Count > 0)
                            {
                                editor._decorations = (await editor.DeltaDecorationsAsync(editor._decorations, value.ToArray())).ToArray();
                            }

                            value.VectorChanged += async (s, cce) =>
                            {
                                var collection = s as IObservableVector<IModelDeltaDecoration>;
                                if (collection != null)
                                {
                                    // Need to recall mutex as this is called from outside of this initial callback setting it up.
                                    using (await editor._mutexLineDecorations.LockAsync())
                                    {
                                        editor._decorations = (await editor.DeltaDecorationsAsync(editor._decorations, collection.ToArray())).ToArray();
                                    }
                                }
                            };
                        }
                    }
                }
            }));

        public static DependencyProperty DecorationsProperty
        {
            get
            {
                return DecorationsPropertyField;
            }
        }
    }
}
