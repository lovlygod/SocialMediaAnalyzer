using System;
using System.Linq;
using System.Windows;

namespace SocialMediaAnalyzerWPF.Services
{
    public enum AppTheme
    {
        Dark,
        Light
    }

    public static class ThemeManager
    {
        private const string DarkThemeUri = "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Dark.xaml";
        private const string LightThemeUri = "pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Light.xaml";
        
        private const string DarkCustomThemeUri = "pack://application:,,,/Resources/DarkTheme.xaml";
        private const string LightCustomThemeUri = "pack://application:,,,/Resources/LightTheme.xaml";

        public static AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;

        public static void SetTheme(AppTheme theme)
        {
            if (CurrentTheme == theme)
                return;

            var resourceDict = Application.Current.Resources.MergedDictionaries;

            var themeDictionariesToRemove = resourceDict.Where(rd =>
                rd.Source != null && (
                    rd.Source.OriginalString.Contains("MaterialDesignTheme") ||
                    rd.Source.OriginalString.Contains("DarkTheme") ||
                    rd.Source.OriginalString.Contains("LightTheme")
                )
            ).ToList();

            foreach (var dict in themeDictionariesToRemove)
            {
                resourceDict.Remove(dict);
            }

            if (theme == AppTheme.Dark)
            {
                resourceDict.Add(new ResourceDictionary { Source = new Uri(DarkThemeUri) });
                resourceDict.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.DeepPurple.xaml") });
                resourceDict.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Grey.xaml") });
                resourceDict.Add(new ResourceDictionary { Source = new Uri(DarkCustomThemeUri) });
            }
            else
            {
                resourceDict.Add(new ResourceDictionary { Source = new Uri(LightThemeUri) });
                resourceDict.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.DeepPurple.xaml") });
                resourceDict.Add(new ResourceDictionary { Source = new Uri("pack://application:,,,/MaterialDesignColors;component/Themes/MaterialDesignColor.Grey.xaml") });
                resourceDict.Add(new ResourceDictionary { Source = new Uri(LightCustomThemeUri) });
            }

            CurrentTheme = theme;
        }

        public static void ToggleTheme()
        {
            SetTheme(CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark);
        }
    }
}