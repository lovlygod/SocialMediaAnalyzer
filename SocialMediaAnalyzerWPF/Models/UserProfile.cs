using System;

namespace SocialMediaAnalyzerWPF.Models
{
    public class UserProfile
    {
        public string Platform { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public int FollowersCount { get; set; }
        public int FollowingCount { get; set; }
        public int PostsCount { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}