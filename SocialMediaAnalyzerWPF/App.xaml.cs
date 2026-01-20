using System.Windows;
using SocialMediaAnalyzerWPF.Localization;

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
            
            // Инициализация локализации
            LocalizationManager.Instance.SetLanguage(LocalizationManager.Instance.CurrentCulture);
        }
    }
}