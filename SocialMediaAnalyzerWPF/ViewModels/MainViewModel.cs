using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaAnalyzerWPF.Commands;
using SocialMediaAnalyzerWPF.Models;
using SocialMediaAnalyzerWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using SocialMediaAnalyzerWPF.Localization;
using System.Globalization;

namespace SocialMediaAnalyzerWPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<SearchResult> _searchResults = new ObservableCollection<SearchResult>();

        [ObservableProperty]
        private ObservableCollection<PhoneNumberResult> _phoneSearchResults = new ObservableCollection<PhoneNumberResult>();

        [ObservableProperty]
        private int _totalPlatformsCount = 0;

        [ObservableProperty]
        private int _foundProfilesCount = 0;

        [ObservableProperty]
        private int _notFoundProfilesCount = 0;

        [ObservableProperty]
        private double _searchTime = 0.0;

        [ObservableProperty]
        private string _myIp = string.Empty;

        [ObservableProperty]
        private string _languageButtonText = "EN";

        [ObservableProperty]
        private string _themeIconKind = "Brightness3";

        [ObservableProperty]
        private bool _isPhoneSearchActive = false;

        private readonly SocialMediaService _socialMediaService;
        private System.Diagnostics.Stopwatch? _stopwatch;
        
        public RelayCommand ShowMyIpCommand { get; }
        public RelayCommand SwitchLanguageCommand { get; }
        public RelayCommand SwitchThemeCommand { get; }
        public RelayCommand PhoneSearchCommand { get; }

        public MainViewModel()
        {
            _socialMediaService = new SocialMediaService();
            ShowMyIpCommand = new RelayCommand(ShowMyIp);
            SwitchLanguageCommand = new RelayCommand(SwitchLanguage);
            SwitchThemeCommand = new RelayCommand(SwitchTheme);
            PhoneSearchCommand = new RelayCommand(PhoneSearchExecute);
            InitializeData();
            
            UpdateLanguageButtonText();
            UpdateThemeButtonText();
            
            LocalizationManager.Instance.LanguageChanged += OnLanguageChanged;
        }

        private void OnLanguageChanged(object? sender, CultureInfo e)
        {
            UpdateLanguageButtonText();
        }

        private void UpdateLanguageButtonText()
        {
            var currentLang = LocalizationManager.Instance.CurrentCulture.Name;
            LanguageButtonText = currentLang.StartsWith("ru") ? "EN" : "RU";
        }

        private void UpdateThemeButtonText()
        {
            ThemeIconKind = ThemeManager.CurrentTheme == AppTheme.Dark ? "WeatherNight" : "WeatherSunny";
        }

        private void SwitchLanguage(object? parameter = null)
        {
            var currentLang = LocalizationManager.Instance.CurrentCulture.Name;
            var newCulture = currentLang.StartsWith("ru") ?
                new CultureInfo("en-US") :
                new CultureInfo("ru-RU");
                
            LocalizationManager.Instance.SetLanguage(newCulture);
        }

        private void SwitchTheme(object? parameter = null)
        {
            ThemeManager.ToggleTheme();
            UpdateThemeButtonText();
        }

        private async void ShowMyIp(object? parameter)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://api.ipify.org/");
                    MyIp = response.Trim();
                    
                    var message = LocalizationManager.Instance.GetLocalizedString("MyIPMessage");
                    var title = LocalizationManager.Instance.GetLocalizedString("InfoMessageTitle");
                    MessageBox.Show(string.Format(message, MyIp), title, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                var message = LocalizationManager.Instance.GetLocalizedString("IPRetrievalError");
                var title = LocalizationManager.Instance.GetLocalizedString("ErrorMessageTitle");
                MessageBox.Show(string.Format(message, ex.Message), title, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InitializeData()
        {
            TotalPlatformsCount = 0;
            FoundProfilesCount = 0;
            NotFoundProfilesCount = 0;
            SearchTime = 0.0;
        }

        public async Task SearchAsync(string? username)
        {
            IsPhoneSearchActive = false;
            SearchResults.Clear();
            PhoneSearchResults.Clear();
            FoundProfilesCount = 0;
            NotFoundProfilesCount = 0;
            TotalPlatformsCount = 0;
            SearchTime = 0.0;
            
            _stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (sender, e) =>
            {
                SearchTime = _stopwatch?.Elapsed.TotalSeconds ?? 0.0;
            };
            timer.Start();
            
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    TotalPlatformsCount = _socialMediaService.GetPlatformCount();
                    OnPropertyChanged(nameof(TotalPlatformsCount));
                    
                    await _socialMediaService.SearchProfileAsync(username, (result) =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            SearchResults.Add(result);
                            
                            if (result.IsFound)
                            {
                                FoundProfilesCount++;
                            }
                            else
                            {
                                NotFoundProfilesCount++;
                            }
                            
                            OnPropertyChanged(nameof(SearchResults));
                            OnPropertyChanged(nameof(FoundProfilesCount));
                            OnPropertyChanged(nameof(NotFoundProfilesCount));
                        });
                    });
                }
            }
            finally
            {
                timer.Stop();
                _stopwatch.Stop();
                SearchTime = _stopwatch.Elapsed.TotalSeconds;
                
                OnPropertyChanged(nameof(SearchTime));
            }
        }

        private async void PhoneSearchExecute(object? parameter)
        {
            string? phoneNumber = parameter as string;
            await PhoneSearchAsync(phoneNumber);
        }
        
        private async Task PhoneSearchAsync(string? phoneNumber)
        {
            IsPhoneSearchActive = true;
            SearchResults.Clear();
            PhoneSearchResults.Clear();
            FoundProfilesCount = 0;
            NotFoundProfilesCount = 0;
            TotalPlatformsCount = 0;
            SearchTime = 0.0;
            
            _stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            var timer = new System.Timers.Timer(100);
            timer.Elapsed += (sender, e) =>
            {
                SearchTime = _stopwatch?.Elapsed.TotalSeconds ?? 0.0;
            };
            timer.Start();
            
            try
            {
                if (!string.IsNullOrEmpty(phoneNumber))
                {
                    TotalPlatformsCount = 1;
                    OnPropertyChanged(nameof(TotalPlatformsCount));
                    
                    var result = await _socialMediaService.SearchByPhoneNumberAsync(phoneNumber);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PhoneSearchResults.Add(result);
                        
                        if (result.IsValid)
                        {
                            FoundProfilesCount++;
                        }
                        else
                        {
                            NotFoundProfilesCount++;
                        }
                        
                        OnPropertyChanged(nameof(PhoneSearchResults));
                        OnPropertyChanged(nameof(FoundProfilesCount));
                        OnPropertyChanged(nameof(NotFoundProfilesCount));
                    });
                }
            }
            finally
            {
                timer.Stop();
                _stopwatch.Stop();
                SearchTime = _stopwatch.Elapsed.TotalSeconds;
                
                OnPropertyChanged(nameof(SearchTime));
            }
        }
    }
}