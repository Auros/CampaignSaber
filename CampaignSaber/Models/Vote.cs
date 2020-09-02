using System;
using System.ComponentModel.DataAnnotations;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace CampaignSaber.Models
{
    public class Vote
    {
        [Key, JsonIgnore]
        public Guid Id { get; set; }

        [Required]
        public User Voter { get; set; }

        [Required]
        public bool IsUpvote { get; set; }

        [Required]
        public string VoterID { get; set; }
    }
}