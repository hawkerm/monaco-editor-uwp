using Windows.ApplicationModel;
using Microsoft.UI.Xaml;

namespace MonacoEditorTestApp
{
    sealed partial class App
    {
        private MainWindow m_window;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            base.OnLaunched(e);

            m_window = new MainWindow();

            m_window.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
        }
    }
}
