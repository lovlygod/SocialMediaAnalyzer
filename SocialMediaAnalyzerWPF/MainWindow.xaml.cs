using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            
            UsernameSearchRadio.Checked += SearchType_Checked;
            PhoneSearchRadio.Checked += SearchType_Checked;
        }

        private void SearchType_Checked(object sender, RoutedEventArgs e)
        {
            if (UsernameSearchRadio.IsChecked == true)
            {
                UsernameTextBox.Visibility = Visibility.Visible;
                PhoneTextBox.Visibility = Visibility.Collapsed;
                UsernameTextBox.Focus();
            }
            else if (PhoneSearchRadio.IsChecked == true)
            {
                UsernameTextBox.Visibility = Visibility.Collapsed;
                PhoneTextBox.Visibility = Visibility.Visible;
                PhoneTextBox.Focus();
            }
        }

        private void OnLanguageChanged(object? sender, CultureInfo e)
        {
            UpdateProgressText();
        }

        private void LanguageSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SwitchLanguageCommand.Execute((object?)null);
        }

        private void ThemeSwitchButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SwitchThemeCommand.Execute((object?)null);
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameSearchRadio.IsChecked == true)
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
            else if (PhoneSearchRadio.IsChecked == true)
            {
                var phoneNumber = PhoneTextBox.Text?.Trim();
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    var message = LocalizationManager.Instance.GetLocalizedString("EmptyPhoneError");
                    var title = LocalizationManager.Instance.GetLocalizedString("ErrorMessageTitle");
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                PerformPhoneSearchAsync(phoneNumber);
            }
        }

        private void PhoneTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var phoneNumber = PhoneTextBox.Text?.Trim();
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    PerformPhoneSearchAsync(phoneNumber);
                }
            }
            else if (e.Key == Key.F12)
            {
                _viewModel.SwitchLanguageCommand.Execute((object?)null);
            }
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
            else if (e.Key == Key.F12)
            {
                _viewModel.SwitchLanguageCommand.Execute((object?)null);
            }
        }

        private async Task PerformSearchAsync(string? username)
        {
            SearchProgressBar.Visibility = Visibility.Visible;
            SearchProgressBar.IsIndeterminate = true;
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
                SearchProgressBar.IsIndeterminate = false;
                SearchProgressBar.Visibility = Visibility.Collapsed;
                UpdateProgressText();
                SearchButton.IsEnabled = true;
            }
        }

        private async void PerformPhoneSearchAsync(string? phoneNumber)
        {
            SearchProgressBar.Visibility = Visibility.Visible;
            SearchProgressBar.IsIndeterminate = true;
            ProgressTextBlock.Text = LocalizationManager.Instance.GetLocalizedString("SearchInProgress");
            ProgressTextBlock.Visibility = Visibility.Visible;
            SearchButton.IsEnabled = false;

            try
            {
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    _viewModel.PhoneSearchCommand.Execute(phoneNumber);
                }
            }
            finally
            {
                SearchProgressBar.IsIndeterminate = false;
                SearchProgressBar.Visibility = Visibility.Collapsed;
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


        private ScrollViewer? GetScrollViewer(DependencyObject? element)
        {
            if (element == null) return null;

            if (element is ScrollViewer scrollViewer)
            {
                return scrollViewer;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                var result = GetScrollViewer(child);
                if (result != null)
                    return result;
            }

            return null;
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