using System;
using System.Collections.Generic;

namespace SocialMediaAnalyzerWPF.Models
{
    public class PlatformData
    {
        public string Name { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string UrlTemplate { get; set; } = string.Empty;
        public string StatusCheckMethod { get; set; } = string.Empty;
        public List<int> SuccessCodes { get; set; } = new List<int>();
        public List<int> FailureCodes { get; set; } = new List<int>();
        public string? SuccessPattern { get; set; }
        public string? FailurePattern { get; set; }
        public int ResponseTimeThresholdMs { get; set; }
        public string Category { get; set; } = string.Empty;
        public int RateLimitDelayMs { get; set; } = 100;
        public PlatformMetadata Metadata { get; set; } = new PlatformMetadata();
    }

    public class PlatformMetadata
    {
        public bool SupportsVerification { get; set; }
        public bool RequiresAuthForDetails { get; set; }
        public int TypicalResponseTimeMs { get; set; }
        public double ReliabilityScore { get; set; }
    }

    public class PlatformsData
    {
        public List<PlatformData> Platforms { get; set; } = new List<PlatformData>();
    }
}