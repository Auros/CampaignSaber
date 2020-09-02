using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class CampaignInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("desc")]
        public string Description { get; set; }
    
        [JsonPropertyName("bigDesc")]
        public string BigDescription { get; set; }
    
        [JsonPropertyName("allUnlocked")]
        public bool AllUnlocked { get; set; }

        [JsonPropertyName("mapPositions")]
        public MapPosition[] MapPositions { get; set; }

        [JsonPropertyName("unlockGate")]
        public UnlockGate[] UnlockGates { get; set; }

        [JsonPropertyName("mapHeight")]
        public int MapHeight { get; set; }

        [JsonPropertyName("backgroundAlpha")]
        public float BackgroundAlpha { get; set; }

        [JsonPropertyName("lightColor")]
        public LightColor LightColor { get; set; }
    }
}