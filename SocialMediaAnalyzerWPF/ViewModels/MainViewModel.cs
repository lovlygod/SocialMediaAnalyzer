using CommunityToolkit.Mvvm.ComponentModel;
using SocialMediaAnalyzerWPF.Models;
using SocialMediaAnalyzerWPF.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Timers;

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

        private readonly SocialMediaService _socialMediaService;
        private System.Diagnostics.Stopwatch? _stopwatch;

        public MainViewModel()
        {
            _socialMediaService = new SocialMediaService();
            InitializeData();
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
                    var results = await _socialMediaService.SearchProfileAsync(username);
                    
                    foreach (var result in results)
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
                    }
                    
                    TotalPlatformsCount = results.Count;
                }
            }
            finally
            {
                timer.Stop();
                _stopwatch.Stop();
                SearchTime = _stopwatch.Elapsed.TotalSeconds;
                
                OnPropertyChanged(nameof(SearchResults));
                OnPropertyChanged(nameof(TotalPlatformsCount));
                OnPropertyChanged(nameof(FoundProfilesCount));
                OnPropertyChanged(nameof(NotFoundProfilesCount));
                OnPropertyChanged(nameof(SearchTime));
            }
        }
    }
}