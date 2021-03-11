using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Microsoft.UI.Xaml;
using Microsoft.System;
using Windows.UI.Core;

namespace Monaco.Helpers
{
    public delegate void ThemeChangedEvent(ThemeListener sender);

    /// <summary>
    /// Class which listens for changes to Application Theme or High Contrast Modes 
    /// and Signals an Event when they occur.
    /// </summary>
    [AllowForWeb]
    public sealed class ThemeListener
    {
        public string CurrentThemeName { get { return CurrentTheme.ToString(); } } // For Web Retrieval

        public ApplicationTheme CurrentTheme { get; set; }
        public bool IsHighContrast { get; set; }

        public DispatcherQueue DispatcherQueue { get; set; }

        public event ThemeChangedEvent ThemeChanged;

        private readonly AccessibilitySettings _accessible = new AccessibilitySettings();
        private readonly UISettings _settings = new UISettings();

        public ThemeListener(DispatcherQueue dispatcherQueue = null)
        {
            CurrentTheme = Application.Current.RequestedTheme;
            IsHighContrast = _accessible.HighContrast;

            DispatcherQueue = dispatcherQueue ?? DispatcherQueue.GetForCurrentThread();

            // Fallback in case either of the above fail, we'll check when we get activated next.
            if (Window.Current != null)
            {
                _accessible.HighContrastChanged += _accessible_HighContrastChanged;
                _settings.ColorValuesChanged += _settings_ColorValuesChanged;

                Window.Current.CoreWindow.Activated += CoreWindow_Activated;
            }
        }

        ~ThemeListener()
        {
            if (Window.Current != null)
            {
                _accessible.HighContrastChanged -= _accessible_HighContrastChanged;
                _settings.ColorValuesChanged -= _settings_ColorValuesChanged;

                Window.Current.CoreWindow.Activated -= CoreWindow_Activated;
            }
        }

        private void _accessible_HighContrastChanged(AccessibilitySettings sender, object args)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("HighContrast Changed");
#endif

            UpdateProperties();
        }

        // Note: This can get called multiple times during HighContrast switch, do we care?
        private void _settings_ColorValuesChanged(UISettings sender, object args)
        {
            // Getting called off thread, so we need to dispatch to request value.
            DispatcherQueue.TryEnqueue(DispatcherQueuePriority.Normal, () =>
            {
                // TODO: This doesn't stop the multiple calls if we're in our faked 'White' HighContrast Mode below.
                if (CurrentTheme != Application.Current.RequestedTheme ||
                    IsHighContrast != _accessible.HighContrast)
                {
#if DEBUG
                    System.Diagnostics.Debug.WriteLine("Color Values Changed");
#endif

                    UpdateProperties();
                }
            });
        }

        private void CoreWindow_Activated(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.WindowActivatedEventArgs args)
        {
            if (CurrentTheme != Application.Current.RequestedTheme ||
                IsHighContrast != _accessible.HighContrast)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("CoreWindow Activated Changed");
#endif

                UpdateProperties();
            }
        }

        /// <summary>
        /// Set our current properties and fire a change notification.
        /// </summary>
        private void UpdateProperties()
        {
            // TODO: Not sure if HighContrastScheme names are localized?
            if (_accessible.HighContrast && _accessible.HighContrastScheme.IndexOf("white", StringComparison.OrdinalIgnoreCase) != -1)
            {
                // If our HighContrastScheme is ON & a lighter one, then we should remain in 'Light' theme mode for Monaco Themes Perspective
                IsHighContrast = false;
                CurrentTheme = ApplicationTheme.Light;
            }
            else
            {
                // Otherwise, we just set to what's in the system as we'd expect.
                IsHighContrast = _accessible.HighContrast;
                CurrentTheme = Application.Current.RequestedTheme;
            }

            ThemeChanged?.Invoke(this);
        }
    }
}
