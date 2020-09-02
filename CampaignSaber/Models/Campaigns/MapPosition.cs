using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class MapPosition
    {
        [JsonPropertyName("childNodes")]
        public int[] ChildNodes { get; set; }

        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        [JsonPropertyName("scale")]
        public float Scale { get; set; }

        [JsonPropertyName("letterPortion")]
        public string LetterPortion { get; set; }

        [JsonPropertyName("numberPortion")]
        public int NumberPortion { get; set; }
    }
}