using SocialMediaAnalyzerWPF.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SocialMediaAnalyzerWPF.Services
{
    public class SocialMediaService
    {
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _platformUrls;

        public SocialMediaService()
        {
            var handler = new HttpClientHandler();
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            
            _platformUrls = new Dictionary<string, string>
            {
                { "Facebook", "https://www.facebook.com/{0}" },
                { "Twitter", "https://twitter.com/{0}" },
                { "Instagram", "https://www.instagram.com/{0}/" },
                { "LinkedIn", "https://www.linkedin.com/in/{0}/" },
                { "YouTube", "https://www.youtube.com/user/{0}" },
                { "TikTok", "https://www.tiktok.com/@{0}" },
                { "Pinterest", "https://www.pinterest.com/{0}/" },
                { "Reddit", "https://www.reddit.com/user/{0}" },
                { "GitHub", "https://github.com/{0}" },
                { "VK", "https://vk.com/{0}" },
                { "Telegram", "https://t.me/{0}" },
                { "Discord", "https://discord.gg/{0}" },
                { "Twitch", "https://www.twitch.tv/{0}" },
                { "Steam", "https://steamcommunity.com/id/{0}" }
            };
        }

        public async Task<List<SearchResult>> SearchProfileAsync(string? username)
        {
            var results = new List<SearchResult>();

            if (string.IsNullOrEmpty(username))
            {
                return results;
            }

            foreach (var platform in _platformUrls)
            {
                var profileUrl = string.Format(platform.Value, username);
                var result = await CheckProfileExistsAsync(platform.Key, username, profileUrl);
                results.Add(result);
                
                await Task.Delay(100);
            }

            return results;
        }

        private async Task<SearchResult> CheckProfileExistsAsync(string platform, string username, string profileUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(profileUrl);
                
                return new SearchResult
                {
                    Platform = platform,
                    Username = username,
                    ProfileUrl = response.IsSuccessStatusCode ? profileUrl : "",
                    Status = response.IsSuccessStatusCode ? "Найден" : "Не найден",
                    IsFound = response.IsSuccessStatusCode
                };
            }
            catch (Exception ex)
            {
                return new SearchResult
                {
                    Platform = platform,
                    Username = username,
                    ProfileUrl = "",
                    Status = $"Ошибка: {ex.Message}",
                    IsFound = false
                };
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}