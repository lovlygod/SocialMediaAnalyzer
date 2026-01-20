using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using SocialMediaAnalyzerWPF.ViewModels;
using SocialMediaAnalyzerWPF.Localization;
using System.Globalization;

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

        private void OnLanguageChanged(object sender, CultureInfo e)
        {
            UpdateProgressText();
        }

        private void LanguageSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SwitchLanguageCommand.Execute(null);
        }

        private void ThemeSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SwitchThemeCommand.Execute(null);
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                var message = LocalizationManager.Instance.GetLocalizedString("EmptyUsernameError");
                var title = LocalizationManager.Instance.GetLocalizedString("ErrorMessageTitle");
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await PerformSearchAsync(username);
        }

        private async void UsernameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var username = UsernameTextBox.Text?.Trim();
                if (!string.IsNullOrEmpty(username))
                {
                    await PerformSearchAsync(username);
                }
            }
            else if (e.Key == Key.F12) // Переключение языка по F12
            {
                _viewModel.SwitchLanguageCommand.Execute(null);
            }
        }

        private async Task PerformSearchAsync(string? username)
        {
            SearchProgressBar.Visibility = Visibility.Visible;
            ProgressTextBlock.Text = LocalizationManager.Instance.GetLocalizedString("SearchInProgress");
            ProgressTextBlock.Visibility = Visibility.Visible;
            SearchButton.IsEnabled = false;

            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    await _viewModel.SearchAsync(username);
                }
            }
            finally
            {
                UpdateProgressText();
                SearchButton.IsEnabled = true;
            }
        }

        private void UpdateProgressText()
        {
            var template = LocalizationManager.Instance.GetLocalizedString("SearchCompletedIn");
            ProgressTextBlock.Text = string.Format(template, _viewModel.SearchTime);
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