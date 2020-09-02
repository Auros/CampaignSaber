using System.Linq;
using CampaignSaber.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CampaignSaber.Authorization;
using CampaignSaber.Models.Upload;
using Microsoft.EntityFrameworkCore;

namespace CampaignSaber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly CampaignSaberContext _campaignSaberContext;

        public VoteController(CampaignSaberContext campaignSaberContext)
        {
            _campaignSaberContext = campaignSaberContext;
        }

        [HttpPost, CSAuthorize]
        public async Task<IActionResult> Vote([FromBody] VotePacket packet)
        {
            User user = HttpContext.Items["User"] as User;
            Campaign campaign = await _campaignSaberContext.Campaigns.FirstOrDefaultAsync(c => packet.CampaignId == c.Id);
            if (campaign == null)
            {
                return NotFound();
            }
            Vote vote = campaign.Stats.Votes.FirstOrDefault(v => v.VoterID == user.Id);
            if (vote == null)
            {
                vote = new Vote
                {
                    VoterID = user.Id,
                    IsUpvote = packet.Value
                };
                campaign.Stats.Votes.Add(vote);
            }
            else
            {
                if ((vote.IsUpvote && packet.Value) || !vote.IsUpvote && !packet.Value)
                {
                    campaign.Stats.Votes.Remove(vote);
                }
                else
                {
                    vote.IsUpvote = packet.Value;
                }
            }
            _campaignSaberContext.Campaigns.Update(campaign);
            await _campaignSaberContext.SaveChangesAsync();
            return NoContent();
        }
    }
}