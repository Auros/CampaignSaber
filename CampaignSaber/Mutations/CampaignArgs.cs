using System;
using System.ComponentModel.DataAnnotations;

namespace CampaignSaber.Mutations
{
    public class CampaignArgs
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Campaign Title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid Id { get; set; }
    }
}