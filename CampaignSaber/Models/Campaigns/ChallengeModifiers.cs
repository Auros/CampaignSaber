using System;
using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    [Serializable]
    public class ChallengeModifiers
    {
        [JsonPropertyName("noFail")]
        public bool NoFail { get; set; }

        [JsonPropertyName("noBombs")]
        public bool NoBombs { get; set; }

        [JsonPropertyName("noArrows")]
        public bool NoArrows { get; set; }

        [JsonPropertyName("fastNotes")]
        public bool FastNotes { get; set; }

        [JsonPropertyName("instaFail")]
        public bool InstaFail { get; set; }

        [JsonPropertyName("ghostNotes")]
        public bool GhostNotes { get; set; }

        [JsonPropertyName("noObstacles")]
        public bool NoObstacles { get; set; }

        [JsonPropertyName("strictAngles")]
        public bool StrictAngles { get; set; }

        [JsonPropertyName("batteryEnergy")]
        public bool BatteryEnergy { get; set; }

        [JsonPropertyName("failOnSaberClash")]
        public bool FailOnSaberClash { get; set; }

        [JsonPropertyName("disappearingArrows")]
        public bool DisappearingArrows { get; set; }

        [JsonPropertyName("songSpeed")]
        public GameplayModifiers.SongSpeed SongSpeed { get; set; }

        [JsonPropertyName("energyType")]
        public GameplayModifiers.EnergyType EnergyType { get; set; }

        [JsonPropertyName("enabledObstacleType")]
        public GameplayModifiers.EnabledObstacleType EnabledObstacleType { get; set; }
    }
}