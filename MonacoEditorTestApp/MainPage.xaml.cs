using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MonacoEditorTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string CodeContent
        {
            get { return (string)GetValue(CodeContentProperty); }
            set { SetValue(CodeContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CodeContentProperty =
            DependencyProperty.Register("CodeContent", typeof(string), typeof(MainPage), new PropertyMetadata(""));

        public MainPage()
        {
            this.CodeContent = "public class Program {\n\tpublic static void Main(string[] args) {\n\t\tConsole.WriteLine(\"Hello, World!\");\n\t}\n}";

            this.InitializeComponent();

            Editor.Loaded += Editor_Loaded;
        }

        private async void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            var languages = await new Monaco.LanguagesHelper(Editor).GetLanguagesAsync();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CodeContent = TextEditor.Text;
        }
    }
}
