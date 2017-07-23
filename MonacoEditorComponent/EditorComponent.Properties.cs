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
        public string Content
        {
            get { return (string)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for HorizontalLayout.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty ContentPropertyField =
            DependencyProperty.Register("Content", typeof(string), typeof(EditorComponent), new PropertyMetadata("", (d, e) => {
                //(d as Canvas)?.InvokeScriptAsync("updateToolbox", new string[] { e.NewValue.ToString() });
                //(d as EditorComponent).CodeChanged?.Invoke(d, e);
            }));

        internal static DependencyProperty ContentProperty
        {
            get
            {
                return ContentPropertyField;
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
