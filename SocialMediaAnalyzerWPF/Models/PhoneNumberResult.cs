using System;

namespace SocialMediaAnalyzerWPF.Models
{
    public class PhoneNumberResult
    {
        public string PhoneNumber { get; set; } = string.Empty;
        
        public bool IsValid { get; set; }
        
        public string Status { get; set; } = string.Empty;
        
        public string Location { get; set; } = string.Empty;
        
        public string Carrier { get; set; } = string.Empty;
        
        public string RegionCode { get; set; } = string.Empty;
        
        public string Timezone { get; set; } = string.Empty;
        
        public string FormattedNumber { get; set; } = string.Empty;
        
        public string NumberType { get; set; } = string.Empty;
        
        public bool IsPossible { get; set; }
        
        public string ErrorMessage { get; set; } = string.Empty;
        
        public string CountryName { get; set; } = string.Empty;
        
        public string Continent { get; set; } = string.Empty;
        
        public string AreaCode { get; set; } = string.Empty;
        
        public string Provider { get; set; } = string.Empty;
    }
}