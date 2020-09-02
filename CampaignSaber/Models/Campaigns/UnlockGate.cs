using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class UnlockGate
    {
        [JsonPropertyName("clearsToPass")]
        public int ClearsToPass { get; set; }

        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }
    }
}