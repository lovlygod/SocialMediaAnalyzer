using SocialMediaAnalyzerWPF.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace SocialMediaAnalyzerWPF.Services
{
    public static class PlatformDataService
    {
        private static PlatformsData? _cachedPlatformsData;
        
        public static PlatformsData GetPlatformsData()
        {
            if (_cachedPlatformsData != null)
            {
                return _cachedPlatformsData;
            }
            
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "platforms.json");
            
            if (!File.Exists(jsonFilePath))
            {
                jsonFilePath = Path.Combine(Environment.CurrentDirectory, "SocialMediaAnalyzerWPF", "Data", "platforms.json");
                
                if (!File.Exists(jsonFilePath))
                {
                    throw new FileNotFoundException($"Platforms JSON file not found: {jsonFilePath}");
                }
            }
            
            string jsonData = File.ReadAllText(jsonFilePath);
            _cachedPlatformsData = JsonConvert.DeserializeObject<PlatformsData>(jsonData);
            
            if (_cachedPlatformsData == null)
            {
                throw new InvalidOperationException("Failed to deserialize platforms data from JSON file.");
            }
            
            return _cachedPlatformsData;
        }
    }
}