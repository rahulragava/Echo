using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace SocialMediaApplication.Util
{
    public static class AppSettings
    {
        public const ElementTheme LightTheme = ElementTheme.Light;
        public const ElementTheme DarkTheme = ElementTheme.Dark;

        const string KeyTheme = "appColourMode";
        //static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        public static ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;
        public static string UserId = LocalSettings.Values["user"]?.ToString();
        public static ApplicationViewTitleBar TitleBar;
       

        static AppSettings()
        {
            LocalSettings = ApplicationData.Current.LocalSettings;
            TitleBar = ApplicationView.GetForCurrentView().TitleBar;
            UserId = LocalSettings.Values["user"]?.ToString();
        }


        /// <summary>
        /// Gets or sets the current app colour setting from memory (light or dark mode).
        /// </summary>
        public static ElementTheme Theme
        {
            get
            {
                // Never set: default theme
                if (LocalSettings.Values[KeyTheme] == null)
                {
                    LocalSettings.Values[KeyTheme] = (int)LightTheme;
                    return LightTheme;
                }
                // Previously set to default theme
                else if ((int)LocalSettings.Values[KeyTheme] == (int)LightTheme)
                {
                    return LightTheme;
                }
                // Previously set to non-default theme
                else
                {
                    return DarkTheme;
                }
            }
            set
            {
                // Error check
                if (value == ElementTheme.Default)
                {
                    throw new System.Exception("Only set the theme to light or dark mode!");
                }
                // Never set
                else if (LocalSettings.Values[KeyTheme] == null)
                {
                    LocalSettings.Values[KeyTheme] = (int)value;

                }
                // No change
                else if ((int)value == (int)LocalSettings.Values[KeyTheme])
                {
                    return;
                }
                // Change
                else
                {
                    LocalSettings.Values[KeyTheme] = (int)value;
                }
            }
        }
    }
}
