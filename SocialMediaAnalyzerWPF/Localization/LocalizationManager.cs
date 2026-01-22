using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SocialMediaAnalyzerWPF.Localization
{
    public class LocalizationManager
    {
        public static LocalizationManager Instance { get; } = new LocalizationManager();

        public event EventHandler<CultureInfo>? LanguageChanged;

        public CultureInfo CurrentCulture { get; private set; }

        private ResourceDictionary? localizedResources;

        private LocalizationManager()
        {
            var systemLanguage = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName;
            
            if (systemLanguage == "ru")
            {
                CurrentCulture = new CultureInfo("ru-RU");
            }
            else
            {
                CurrentCulture = new CultureInfo("en-US");
            }
            
            LoadLocalizedResources();
        }

        public void SetLanguage(CultureInfo culture)
        {
            CurrentCulture = culture;
            LoadLocalizedResources();
            LanguageChanged?.Invoke(this, CurrentCulture);
        }

        private void LoadLocalizedResources()
        {
            if (localizedResources != null)
            {
                Application.Current.Resources.MergedDictionaries.Remove(localizedResources);
            }

            var resourceFileName = $"/Localization/Strings.{CurrentCulture.Name}.xaml";
            
            try
            {
                var uri = new Uri(resourceFileName, UriKind.Relative);
                localizedResources = new ResourceDictionary() { Source = uri };
                Application.Current.Resources.MergedDictionaries.Add(localizedResources);
            }
            catch (Exception)
            {
                if (CurrentCulture.Name != "en-US")
                {
                    var fallbackUri = new Uri("/Localization/Strings.en-US.xaml", UriKind.Relative);
                    localizedResources = new ResourceDictionary() { Source = fallbackUri };
                    Application.Current.Resources.MergedDictionaries.Add(localizedResources);
                    CurrentCulture = new CultureInfo("en-US");
                }
            }
        }

        public string GetLocalizedString(string key)
        {
            if (localizedResources?.Contains(key) == true)
            {
                return localizedResources[key]?.ToString() ?? key;
            }
            return key;
        }
    }
}