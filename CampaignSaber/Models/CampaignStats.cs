using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignSaber.Models
{
    public class CampaignStats
    {
        [Required]
        public int DownloadCount { get; set; }
        
        [Required]
        public List<Vote> Votes { get; set; } = new List<Vote>();

        [NotMapped]
        public int Upvotes => Votes.Count(v => v.IsUpvote);

        [NotMapped]
        public int Downvotes => Votes.Count(v => !v.IsUpvote);

        [NotMapped]
        public float Rating
        {
            get
            {
                int dem = Upvotes + Downvotes;
                if (dem == 0) return 0f;
                return (float)Upvotes / dem;
            }
        }
    }
}