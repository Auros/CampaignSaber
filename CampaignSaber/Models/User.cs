using CampaignSaber.Models.Discord;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignSaber.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required, Column(TypeName = "jsonb")]
        public DiscordUser Profile { get; set; }
    }
}