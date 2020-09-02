using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignSaber.Models.Campaigns
{
    public class UnlockableItem
    {
        [JsonPropertyName("type")]
        public UnlockableType Type { get; set; }
        
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
    public enum UnlockableType
    {
        SABER,
        AVATAR,
        PLATFORM,
        NOTE
    }
}