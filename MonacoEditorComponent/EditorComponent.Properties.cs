using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Monaco
{
    partial class EditorComponent
    {
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty TextPropertyField =
            DependencyProperty.Register("Text", typeof(string), typeof(EditorComponent), new PropertyMetadata("", (d, e) => {
                //(d as Canvas)?.InvokeScriptAsync("updateToolbox", new string[] { e.NewValue.ToString() });
                //(d as EditorComponent).CodeChanged?.Invoke(d, e);

                (d as EditorComponent)?.InvokeScriptAsync("updateContent", e.NewValue.ToString());
            }));

        public static DependencyProperty TextProperty
        {
            get
            {
                return TextPropertyField;
            }
        }

        public string CodeLanguage
        {
            get { return (string)GetValue(CodeLanguageProperty); }
            set { SetValue(CodeLanguageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty CodeLanguagePropertyField =
            DependencyProperty.Register("CodeLanguage", typeof(string), typeof(EditorComponent), new PropertyMetadata("csharp", (d, e) => {
                //(d as Canvas)?.InvokeScriptAsync("updateToolbox", new string[] { e.NewValue.ToString() });
                //(d as EditorComponent).CodeChanged?.Invoke(d, e);
            }));

        internal static DependencyProperty CodeLanguageProperty
        {
            get
            {
                return CodeLanguagePropertyField;
            }
        }
    }
}
