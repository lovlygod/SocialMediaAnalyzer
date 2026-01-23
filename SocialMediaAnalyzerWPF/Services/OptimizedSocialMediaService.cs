using SocialMediaAnalyzerWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace SocialMediaAnalyzerWPF.Services
{
    public class OptimizedSocialMediaService
    {
        private readonly HttpClient _httpClient;
        private readonly List<PlatformData> _platforms;
        private readonly PhoneLookupService _phoneLookupService;

        public OptimizedSocialMediaService()
        {
            var handler = new HttpClientHandler();
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            _platforms = PlatformDataService.GetPlatformsData().Platforms
                .OrderByDescending(p => p.Metadata.ReliabilityScore)
                .ThenBy(p => p.Metadata.TypicalResponseTimeMs)
                .ToList();
            
            _phoneLookupService = new PhoneLookupService();
        }

        public int GetPlatformCount()
        {
            return _platforms.Count;
        }

        public async Task SearchProfileAsync(string? username, Action<SearchResult> onResultReceived)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            var platformGroups = _platforms.GroupBy(p => p.Category).ToList();

            var tasks = new List<Task>();

            foreach (var group in platformGroups)
            {
                var groupTask = Task.Run(async () =>
                {
                    foreach (var platform in group.OrderBy(p => p.Metadata.ReliabilityScore).ThenBy(p => p.Metadata.TypicalResponseTimeMs))
                    {
                        var profileUrl = platform.UrlTemplate.Replace("{username}", username);
                        var result = await CheckProfileExistsAsync(platform, username, profileUrl);
                        onResultReceived(result);

                        await Task.Delay(platform.RateLimitDelayMs);
                    }
                });
                tasks.Add(groupTask);
            }

            await Task.WhenAll(tasks);
        }

        public async Task<List<SearchResult>> SearchProfileAsync(string? username)
        {
            var results = new List<SearchResult>();

            if (string.IsNullOrEmpty(username))
            {
                return results;
            }

            foreach (var platform in _platforms)
            {
                var profileUrl = platform.UrlTemplate.Replace("{username}", username);
                var result = await CheckProfileExistsAsync(platform, username, profileUrl);
                results.Add(result);
                
                await Task.Delay(platform.RateLimitDelayMs);
            }

            return results;
        }

        public async Task<PhoneNumberResult> SearchByPhoneNumberAsync(string phoneNumber)
        {
            return await _phoneLookupService.LookupPhoneNumberAsync(phoneNumber);
        }

        private async Task<SearchResult> CheckProfileExistsAsync(PlatformData platform, string username, string profileUrl)
        {
            try
            {
                using (var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(platform.ResponseTimeThresholdMs)))
                {
                    var response = await _httpClient.GetAsync(profileUrl, cts.Token);
                    
                    bool isFound = false;
                    
                    if (platform.StatusCheckMethod == "http_status_code")
                    {
                        if (platform.SuccessCodes.Contains((int)response.StatusCode))
                        {
                            isFound = true;
                        }
                        else if (platform.FailureCodes.Contains((int)response.StatusCode))
                        {
                            isFound = false;
                        }
                        else
                        {
                            isFound = false;
                        }
                    }
                    
                    return new SearchResult
                    {
                        Platform = platform.Name,
                        Username = username,
                        ProfileUrl = isFound ? profileUrl : "",
                        Status = isFound ?
                            Localization.LocalizationManager.Instance.GetLocalizedString("StatusFound") :
                            Localization.LocalizationManager.Instance.GetLocalizedString("StatusNotFound"),
                        IsFound = isFound
                    };
                }
            }
            catch (OperationCanceledException)
            {
                return new SearchResult
                {
                    Platform = platform.Name,
                    Username = username,
                    ProfileUrl = "",
                    Status = Localization.LocalizationManager.Instance.GetLocalizedString("StatusTimeoutError"),
                    IsFound = false
                };
            }
            catch (HttpRequestException httpEx)
            {
                return new SearchResult
                {
                    Platform = platform.Name,
                    Username = username,
                    ProfileUrl = "",
                    Status = string.Format(Localization.LocalizationManager.Instance.GetLocalizedString("StatusHttpError"), httpEx.Message),
                    IsFound = false
                };
            }
            catch (Exception ex)
            {
                return new SearchResult
                {
                    Platform = platform.Name,
                    Username = username,
                    ProfileUrl = "",
                    Status = string.Format(Localization.LocalizationManager.Instance.GetLocalizedString("StatusGeneralError"), ex.Message),
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