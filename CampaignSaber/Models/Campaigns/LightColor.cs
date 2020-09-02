using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class LightColor
    {
        [JsonPropertyName("r")]
        public float R { get; set; }

        [JsonPropertyName("g")]
        public float G { get; set; }

        [JsonPropertyName("b")]
        public float B { get; set; }
    }
}