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
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
            
            _platformUrls = new Dictionary<string, string>
            {
                { "AboutMe", "https://about.me/{0}" },
                { "Behance", "https://www.behance.net/{0}" },
                { "Bitbucket", "https://bitbucket.org/{0}" },
                { "Blogger", "https://{0}.blogspot.com" },
                { "BuyMeACoffee", "https://buymeacoff.ee/{0}" },
                { "CashApp", "https://cash.app/${0}" },
                { "CodePen", "https://codepen.io/{0}" },
                { "Dribbble", "https://dribbble.com/{0}" },
                { "Discord", "https://discord.com/users/{0}" },
                { "Facebook", "https://www.facebook.com/{0}" },
                { "Flickr", "https://www.flickr.com/photos/{0}" },
                { "Foursquare", "https://foursquare.com/user/{0}" },
                { "GitHub", "https://github.com/{0}" },
                { "GitLab", "https://gitlab.com/{0}" },
                { "Goodreads", "https://www.goodreads.com/user/show/{0}" },
                { "Gravatar", "https://en.gravatar.com/{0}" },
                { "HackerNews", "https://news.ycombinator.com/user?id={0}" },
                { "Imgur", "https://imgur.com/user/{0}" },
                { "Instagram", "https://www.instagram.com/{0}/" },
                { "LastFM", "https://www.last.fm/user/{0}" },
                { "LinkedIn", "https://www.linkedin.com/in/{0}/" },
                { "Linktree", "https://linktr.ee/{0}" },
                { "Mastodon", "https://mastodon.social/@{0}" },
                { "Meetup", "https://www.meetup.com/members/{0}" },
                { "Medium", "https://medium.com/@{0}" },
                { "Myspace", "https://myspace.com/{0}" },
                { "OnlyFans", "https://onlyfans.com/{0}" },
                { "Pastebin", "https://pastebin.com/u/{0}" },
                { "Patreon", "https://www.patreon.com/{0}" },
                { "Periscope", "https://www.periscope.tv/{0}" },
                { "Pinterest", "https://www.pinterest.com/{0}/" },
                { "Quora", "https://www.quora.com/profile/{0}" },
                { "Reddit", "https://www.reddit.com/user/{0}" },
                { "Roblox", "https://www.roblox.com/user.aspx?username={0}" },
                { "Shopify", "https://{0}.shopify.com" },
                { "Signal", "https://signal.me/#p/{0}" },
                { "Skype", "https://secure.skype.com/userprofile/{0}" },
                { "Slack", "https://{0}.slack.com" },
                { "Snapchat", "https://www.snapchat.com/add/{0}" },
                { "SoundCloud", "https://soundcloud.com/{0}" },
                { "Spotify", "https://open.spotify.com/user/{0}" },
                { "StackOverflow", "https://stackoverflow.com/users/{0}" },
                { "Steam", "https://steamcommunity.com/id/{0}" },
                { "Telegram", "https://t.me/{0}" },
                { "TikTok", "https://www.tiktok.com/@{0}" },
                { "TripAdvisor", "https://www.tripadvisor.com/members/{0}" },
                { "Tumblr", "https://{0}.tumblr.com" },
                { "Twitch", "https://www.twitch.tv/{0}" },
                { "Twitter", "https://twitter.com/{0}" },
                { "VK", "https://vk.com/{0}" },
                { "Vimeo", "https://vimeo.com/{0}" },
                { "Wikipedia", "https://wikipedia.org/wiki/User:{0}" },
                { "WordPress", "https://{0}.wordpress.com" },
                { "Xing", "https://www.xing.com/profile/{0}" },
                { "YouTube", "https://www.youtube.com/user/{0}" }
            };
        }

        public int GetPlatformCount()
        {
            return _platformUrls.Count;
        }

        public async Task SearchProfileAsync(string? username, Action<SearchResult> onResultReceived)
        {
            if (string.IsNullOrEmpty(username))
            {
                return;
            }

            var tasks = new List<Task>();

            foreach (var platform in _platformUrls)
            {
                var task = Task.Run(async () =>
                {
                    var profileUrl = string.Format(platform.Value, username);
                    var result = await CheckProfileExistsAsync(platform.Key, username, profileUrl);
                    onResultReceived(result);
                });
                tasks.Add(task);

                await Task.Delay(100);
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
                // Use a cancellation token to control request timeout
                using (var cts = new CancellationTokenSource(_httpClient.Timeout))
                {
                    var response = await _httpClient.GetAsync(profileUrl, cts.Token);
                    
                    return new SearchResult
                    {
                        Platform = platform,
                        Username = username,
                        ProfileUrl = response.IsSuccessStatusCode ? profileUrl : "",
                        Status = response.IsSuccessStatusCode ? "Найден" : "Не найден",
                        IsFound = response.IsSuccessStatusCode
                    };
                }
            }
            catch (OperationCanceledException)
            {
                return new SearchResult
                {
                    Platform = platform,
                    Username = username,
                    ProfileUrl = "",
                    Status = "Ошибка: Время ожидания истекло",
                    IsFound = false
                };
            }
            catch (HttpRequestException httpEx)
            {
                return new SearchResult
                {
                    Platform = platform,
                    Username = username,
                    ProfileUrl = "",
                    Status = $"Ошибка HTTP: {httpEx.Message}",
                    IsFound = false
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