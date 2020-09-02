using System;

namespace CampaignSaber.Models.Upload
{
    public class VotePacket
    {
        public Guid CampaignId { get; set; }

        public bool Value { get; set; }
    }
}