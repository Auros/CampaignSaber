using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CampaignSaber.Models.Discord
{
    public class DiscordUser
    {
        private string _avatar;

        [Key, JsonPropertyName("id"), Required]
        public string Id { get; set; }

        [JsonPropertyName("username"), Required]
        public string Username { get; set; }

        [JsonPropertyName("discriminator"), Required]
        public string Discriminator { get; set; }
        
        [JsonPropertyName("avatar"), Required]
        public string Avatar
        {
            get => _avatar;
            set => _avatar = value.StartsWith("http") ? value : ("https://cdn.discordapp.com/avatars/" + Id + "/" + value + (value.Substring(0, 2) == "a_" ? ".gif" : ".png"));
        }
    }
}