using System;
using System.IO;
using CampaignSaber.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampaignSaber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        private readonly CampaignSaberContext _campaignSaberContext;

        public DownloadController(CampaignSaberContext campaignSaberContext)
        {
            _campaignSaberContext = campaignSaberContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var campaign = await _campaignSaberContext.Campaigns.FirstOrDefaultAsync(c => c.Id == id);
            if (campaign == null)
            {
                return NotFound();
            }
            campaign.Stats.DownloadCount++;
            _campaignSaberContext.Campaigns.Update(campaign);
            var ddu = System.IO.File.OpenRead(campaign.DirectDownloadURL.TrimStart('/'));
            using var ms = new MemoryStream();
            await ddu.CopyToAsync(ms);
            await _campaignSaberContext.SaveChangesAsync();
            return File(fileContents: ms.ToArray(), contentType: "application/octet-stream", fileDownloadName: campaign.Title + ".zip");
        }
    }
}