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

namespace SocialMediaAnalyzerWPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<SearchResult> _searchResults = new ObservableCollection<SearchResult>();

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

        private readonly SocialMediaService _socialMediaService;
        private System.Diagnostics.Stopwatch? _stopwatch;
        
        public RelayCommand ShowMyIpCommand { get; }

        public MainViewModel()
        {
            _socialMediaService = new SocialMediaService();
            ShowMyIpCommand = new RelayCommand(ShowMyIp);
            InitializeData();
        }

        private async void ShowMyIp(object parameter)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetStringAsync("https://api.ipify.org/");
                    MyIp = response.Trim();
                    
                    MessageBox.Show($"Ваш IP-адрес: {MyIp}", "Мой IP", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении IP-адреса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            SearchResults.Clear();
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
    }
}