using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignSaber.Models
{
    public class Campaign
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Hash { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public User Uploader { get; set; }

        public string CoverURL { get; set; }

        [Required]
        public DateTime Uploaded { get; set; }

        [Required]
        public string UploaderId { get; set; }

        public string Description { get; set; }

        [Required]
        public string DownloadURL { get; set; }

        [Required, Column(TypeName = "jsonb")]
        public CampaignStats Stats { get; set; }

        [Required]
        public string DirectDownloadURL { get; set; }

        [Required, Column(TypeName = "jsonb")]
        public CampaignMetadata Metadata { get ; set; }
    }
}