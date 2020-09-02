using System.Text.Json.Serialization;

namespace CampaignSaber.Models.Campaigns
{
    public class Challenge
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("songid")]
        public string SongID { get; set; }

        [JsonPropertyName("customDownloadURL")]
        public string CustomDownloadURL { get; set; }

        [JsonPropertyName("characteristic")]
        public string Characteristic { get; set; }

        [JsonPropertyName("difficulty")]
        public BeatmapDifficulty Difficulty { get; set; }

        [JsonPropertyName("modifiers")]
        public ChallengeModifiers Modifiers { get; set; }

        [JsonPropertyName("requirements")]
        public ChallengeRequirement[] Requirements { get; set; }

        [JsonPropertyName("challengeInfo")]
        public ChallengeInfo ChallengeInfo { get; set; }

        [JsonPropertyName("unlockableItems")]
        public UnlockableItem[] UnlockableItems { get; set; }

        [JsonPropertyName("unlockMap")]
        public bool UnlockMap { get; set; }

        public string NodeName { get; set; }
    }
}