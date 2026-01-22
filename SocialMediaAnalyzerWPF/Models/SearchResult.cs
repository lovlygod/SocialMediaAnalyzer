using System;

namespace SocialMediaAnalyzerWPF.Models
{
    public class SearchResult
    {
        public string Platform { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string ProfileUrl { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime SearchDate { get; set; } = DateTime.Now;
        public bool IsFound { get; set; }
    }
}