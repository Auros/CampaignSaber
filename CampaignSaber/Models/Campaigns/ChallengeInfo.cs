using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CampaignSaber.Models.Campaigns
{
    public class ChallengeInfo
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("showEverytime")]
        public bool ShowEverytime { get; set; }

        [JsonPropertyName("segments")]
        public IList<InfoSegment> Segments { get; set; }
    }
}