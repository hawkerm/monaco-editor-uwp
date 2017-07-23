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
        public MainPage()
        {
            this.InitializeComponent();

            Editor.Content = "public class Program {\n\tpublic static void Main(string[] args) {\n\t\tConsole.WriteLine(\"Hello, World!\");\n\t}\n}";

            Editor.Loaded += Editor_Loaded;
        }

        private async void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            var languages = await new Monaco.LanguagesHelper(Editor).GetLanguagesAsync();

        }
    }
}
