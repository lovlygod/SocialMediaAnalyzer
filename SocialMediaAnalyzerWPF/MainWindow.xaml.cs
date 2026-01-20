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
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var username = UsernameTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя для поиска.",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
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
        }

        private async Task PerformSearchAsync(string? username)
        {
            SearchProgressBar.Visibility = Visibility.Visible;
            ProgressTextBlock.Text = "Поиск...";
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
                SearchProgressBar.Visibility = Visibility.Collapsed;
                ProgressTextBlock.Text = $"Поиск завершен за {_viewModel.SearchTime:F2} секунд";
                
                SearchButton.IsEnabled = true;
            }
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
                MessageBox.Show($"Не удалось открыть ссылку: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}