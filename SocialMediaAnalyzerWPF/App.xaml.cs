using System.Windows;
using SocialMediaAnalyzerWPF.Localization;
using SocialMediaAnalyzerWPF.Services;

namespace SocialMediaAnalyzerWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            ThemeManager.SetTheme(AppTheme.Dark);
            
            LocalizationManager.Instance.SetLanguage(LocalizationManager.Instance.CurrentCulture);
        }
    }
}