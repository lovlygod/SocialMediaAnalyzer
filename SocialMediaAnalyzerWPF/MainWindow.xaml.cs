using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using SocialMediaAnalyzerWPF.ViewModels;
using SocialMediaAnalyzerWPF.Localization;

namespace SocialMediaAnalyzerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;
            
            LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged(object? sender, CultureInfo e)
        {
            // Обновление текста не требуется, так как используется привязка к ресурсам
        }

        private void LanguageSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SwitchLanguageCommand.Execute((object?)null);
        }

        private void ThemeSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SwitchThemeCommand.Execute((object?)null);
        }

        private void UsernameSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ShowUsernameSearchPanel();
        }

        private void PhoneSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPhoneSearchPanel();
        }

        private void EmailSearchButton_Click(object sender, RoutedEventArgs e)
        {
            ShowEmailSearchPanel();
        }

        private void ShowUsernameSearchPanel()
        {
            CleanupPreviousView();
            WelcomeScreen.Visibility = Visibility.Collapsed;
            GetUsernameSearchControl().Visibility = Visibility.Visible;
            GetPhoneSearchControl().Visibility = Visibility.Collapsed;
            GetEmailSearchControl().Visibility = Visibility.Collapsed;
            
            BackButton.Visibility = Visibility.Visible;
            MainTitle.Text = LocalizationManager.Instance.GetLocalizedString("UsernameSearchTitle");
        }

        private void ShowPhoneSearchPanel()
        {
            CleanupPreviousView();
            WelcomeScreen.Visibility = Visibility.Collapsed;
            GetUsernameSearchControl().Visibility = Visibility.Collapsed;
            GetPhoneSearchControl().Visibility = Visibility.Visible;
            GetEmailSearchControl().Visibility = Visibility.Collapsed;
            
            BackButton.Visibility = Visibility.Visible;
            MainTitle.Text = LocalizationManager.Instance.GetLocalizedString("PhoneSearchTitle");
        }

        private void ShowEmailSearchPanel()
        {
            CleanupPreviousView();
            WelcomeScreen.Visibility = Visibility.Collapsed;
            GetUsernameSearchControl().Visibility = Visibility.Collapsed;
            GetPhoneSearchControl().Visibility = Visibility.Collapsed;
            GetEmailSearchControl().Visibility = Visibility.Visible;
            
            BackButton.Visibility = Visibility.Visible;
            MainTitle.Text = LocalizationManager.Instance.GetLocalizedString("EmailSearchTitle");
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Вернуться к главному экрану
            CleanupPreviousView();
            WelcomeScreen.Visibility = Visibility.Visible;
            GetUsernameSearchControl().Visibility = Visibility.Collapsed;
            GetPhoneSearchControl().Visibility = Visibility.Collapsed;
            GetEmailSearchControl().Visibility = Visibility.Collapsed;
            
            BackButton.Visibility = Visibility.Collapsed;
            MainTitle.Text = LocalizationManager.Instance.GetLocalizedString("AppName");
        }

        private void EmailSearchControl_BackButtonClicked(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
        
        // Метод для очистки предыдущего элемента управления перед переключением
        private void CleanupPreviousView()
        {
            var usernameControl = this.FindName("UsernameSearchControl") as UserControls.UsernameSearchControl;
            var phoneControl = this.FindName("PhoneSearchControl") as UserControls.PhoneSearchControl;
            
            if (UsernameSearchControl.Visibility == Visibility.Visible && usernameControl != null)
            {
                usernameControl.Cleanup();
            }
            else if (PhoneSearchControl.Visibility == Visibility.Visible && phoneControl != null)
            {
                phoneControl.Cleanup();
            }
        }

        // Методы для получения элементов управления
        private UserControl GetUsernameSearchControl()
        {
            return (UserControl)FindName("UsernameSearchControl");
        }

        private UserControl GetPhoneSearchControl()
        {
            return (UserControl)FindName("PhoneSearchControl");
        }

        private UserControl GetEmailSearchControl()
        {
            return (UserControl)FindName("EmailSearchControl");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fadeInAnimation = FindResource("FadeInAnimation") as Storyboard;
            if (fadeInAnimation != null)
            {
                fadeInAnimation.Begin(this);
            }
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            animation.BeginTime = TimeSpan.FromMilliseconds(300);
            (sender as TextBox)?.BeginAnimation(OpacityProperty, animation);
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            var animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.5));
            animation.BeginTime = TimeSpan.FromMilliseconds(500);
            (sender as ListView)?.BeginAnimation(OpacityProperty, animation);
        }

        private void GitHubLink_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/lovlygod",
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                var message = LocalizationManager.Instance.GetLocalizedString("LinkOpenError");
                var title = LocalizationManager.Instance.GetLocalizedString("ErrorMessageTitle");
                MessageBox.Show(string.Format(message, ex.Message), title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}