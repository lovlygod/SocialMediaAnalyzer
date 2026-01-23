using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using SocialMediaAnalyzerWPF.ViewModels;
using SocialMediaAnalyzerWPF.Localization;
using System.Globalization;

namespace SocialMediaAnalyzerWPF.UserControls
{
    /// <summary>
    /// Interaction logic for PhoneSearchControl.xaml
    /// </summary>
    public partial class PhoneSearchControl : UserControl
    {
        public static readonly DependencyProperty PhoneSearchResultsProperty =
            DependencyProperty.Register("PhoneSearchResults", typeof(System.Collections.ObjectModel.ObservableCollection<Models.PhoneNumberResult>), 
            typeof(PhoneSearchControl), new PropertyMetadata(null));

        public static readonly DependencyProperty TotalPlatformsCountProperty =
            DependencyProperty.Register("TotalPlatformsCount", typeof(int), 
            typeof(PhoneSearchControl), new PropertyMetadata(0));

        public static readonly DependencyProperty FoundProfilesCountProperty =
            DependencyProperty.Register("FoundProfilesCount", typeof(int), 
            typeof(PhoneSearchControl), new PropertyMetadata(0));

        public static readonly DependencyProperty NotFoundProfilesCountProperty =
            DependencyProperty.Register("NotFoundProfilesCount", typeof(int), 
            typeof(PhoneSearchControl), new PropertyMetadata(0));

        public static readonly DependencyProperty SearchTimeProperty =
            DependencyProperty.Register("SearchTime", typeof(double), 
            typeof(PhoneSearchControl), new PropertyMetadata(0.0));

        public System.Collections.ObjectModel.ObservableCollection<Models.PhoneNumberResult> PhoneSearchResults
        {
            get { return (System.Collections.ObjectModel.ObservableCollection<Models.PhoneNumberResult>)GetValue(PhoneSearchResultsProperty); }
            set { SetValue(PhoneSearchResultsProperty, value); }
        }

        public int TotalPlatformsCount
        {
            get { return (int)GetValue(TotalPlatformsCountProperty); }
            set { SetValue(TotalPlatformsCountProperty, value); }
        }

        public int FoundProfilesCount
        {
            get { return (int)GetValue(FoundProfilesCountProperty); }
            set { SetValue(FoundProfilesCountProperty, value); }
        }

        public int NotFoundProfilesCount
        {
            get { return (int)GetValue(NotFoundProfilesCountProperty); }
            set { SetValue(NotFoundProfilesCountProperty, value); }
        }

        public double SearchTime
        {
            get { return (double)GetValue(SearchTimeProperty); }
            set { SetValue(SearchTimeProperty, value); }
        }

        private readonly MainViewModel _viewModel;

        public PhoneSearchControl()
        {
            InitializeComponent();
            
            _viewModel = new MainViewModel();
            
            DataContext = _viewModel;
            
            PhoneSearchResults = _viewModel.PhoneSearchResults;
            TotalPlatformsCount = _viewModel.TotalPlatformsCount;
            FoundProfilesCount = _viewModel.FoundProfilesCount;
            NotFoundProfilesCount = _viewModel.NotFoundProfilesCount;
            SearchTime = _viewModel.SearchTime;
            
            LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
            
            _viewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewModel.TotalPlatformsCount):
                    TotalPlatformsCount = _viewModel.TotalPlatformsCount;
                    break;
                case nameof(MainViewModel.FoundProfilesCount):
                    FoundProfilesCount = _viewModel.FoundProfilesCount;
                    break;
                case nameof(MainViewModel.NotFoundProfilesCount):
                    NotFoundProfilesCount = _viewModel.NotFoundProfilesCount;
                    break;
                case nameof(MainViewModel.SearchTime):
                    SearchTime = _viewModel.SearchTime;
                    break;
            }
        }

        private void OnLanguageChanged(object? sender, CultureInfo e)
        {
            UpdateProgressText();
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var phoneNumber = PhoneTextBox.Text?.Trim();
            if (string.IsNullOrEmpty(phoneNumber))
            {
                var message = LocalizationManager.Instance.GetLocalizedString("EmptyPhoneError");
                var title = LocalizationManager.Instance.GetLocalizedString("ErrorMessageTitle");
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            await PerformSearchAsync(phoneNumber);
        }

        private async void PhoneTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var phoneNumber = PhoneTextBox.Text?.Trim();
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    await PerformSearchAsync(phoneNumber);
                }
            }
            else if (e.Key == Key.F12)
            {
                _viewModel.SwitchLanguageCommand.Execute(null);
            }
        }

        private async Task PerformSearchAsync(string? phoneNumber)
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
                    
                    TotalPlatformsCount = _viewModel.TotalPlatformsCount;
                    FoundProfilesCount = _viewModel.FoundProfilesCount;
                    NotFoundProfilesCount = _viewModel.NotFoundProfilesCount;
                    SearchTime = _viewModel.SearchTime;
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
        
        public void Cleanup()
        {
            _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            LocalizationManager.Instance.LanguageChanged -= OnLanguageChanged;
        }
    }
}