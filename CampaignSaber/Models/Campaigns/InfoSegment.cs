using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class InfoSegment
    {

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("imageName")]
        public string ImageName { get; set; }

        [JsonPropertyName("hasSeperator")]
        public bool HasSeperator { get; set; }
    }
}