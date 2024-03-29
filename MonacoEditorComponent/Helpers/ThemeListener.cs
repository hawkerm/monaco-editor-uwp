﻿using Microsoft.Toolkit.Uwp;
using System;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Monaco.Helpers
{
    public delegate void ThemeChangedEvent(ThemeListener sender);

    /// <summary>
    /// Class which listens for changes to Application Theme or High Contrast Modes 
    /// and Signals an Event when they occur.
    /// </summary>
    [AllowForWeb]
    public sealed class ThemeListener // This is a copy of the Toolkit ThemeListener, for some reason if we try and use it directly it's not read by the WebView
    {
        private readonly DispatcherQueue _queue;

        public string CurrentThemeName { get { return CurrentTheme.ToString(); } } // For Web Retrieval

        public ApplicationTheme CurrentTheme { get; set; }
        public bool IsHighContrast { get; set; }

        public event ThemeChangedEvent ThemeChanged;

        private readonly AccessibilitySettings _accessible = new AccessibilitySettings();
        private readonly UISettings _settings = new UISettings();

        public ThemeListener() : this(null) { }

        public ThemeListener(DispatcherQueue queue)
        {
            _queue = queue ?? DispatcherQueue.GetForCurrentThread();

            CurrentTheme = Application.Current.RequestedTheme;
            IsHighContrast = _accessible.HighContrast;

            _accessible.HighContrastChanged += _accessible_HighContrastChanged;
            _settings.ColorValuesChanged += _settings_ColorValuesChanged;

            // Fallback in case either of the above fail, we'll check when we get activated next.
            Window.Current.CoreWindow.Activated += CoreWindow_Activated; 
        }

        ~ThemeListener()
        {
            _accessible.HighContrastChanged -= _accessible_HighContrastChanged;
            _settings.ColorValuesChanged -= _settings_ColorValuesChanged;

            Window.Current.CoreWindow.Activated -= CoreWindow_Activated;
        }

        private void _accessible_HighContrastChanged(AccessibilitySettings sender, object args)
        {
            #if DEBUG
            Debug.WriteLine("HighContrast Changed");
            #endif

            UpdateProperties();
        }

        // Note: This can get called multiple times during HighContrast switch, do we care?
        private async void _settings_ColorValuesChanged(UISettings sender, object args)
        {
            // Getting called off thread, so we need to dispatch to request value.
            await _queue.EnqueueAsync(() =>
            {
                // TODO: This doesn't stop the multiple calls if we're in our faked 'White' HighContrast Mode below.
                if (CurrentTheme != Application.Current.RequestedTheme ||
                    IsHighContrast != _accessible.HighContrast)
                {
                    #if DEBUG
                    Debug.WriteLine("Color Values Changed");
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
                Debug.WriteLine("CoreWindow Activated Changed");
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
