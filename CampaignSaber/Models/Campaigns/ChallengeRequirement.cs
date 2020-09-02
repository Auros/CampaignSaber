using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class ChallengeRequirement
    {
        [JsonPropertyName("isMax")]
        public bool IsMax { get; set; }
        
        [JsonPropertyName("type")]
        public string Type { get; set; }
        
        [JsonPropertyName("count")]
        public int Count { get; set; }
    }
}