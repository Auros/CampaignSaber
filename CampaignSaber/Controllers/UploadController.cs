using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Text.Json;
using CampaignSaber.Models;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using CampaignSaber.Models.Upload;
using CampaignSaber.Authorization;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using CampaignSaber.Models.Campaigns;

namespace CampaignSaber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly CampaignSaberContext _campaignSaberContext;

        public UploadController(CampaignSaberContext campaignSaberContext)
        {
            _campaignSaberContext = campaignSaberContext;
        }

        [HttpPost]
        [CSAuthorize]
        [RequestSizeLimit(15000000)]
        public async Task<IActionResult> Upload([FromForm] UploadCampaign body)
        {
            User user = HttpContext.Items["User"] as User;
            
            if (body.Campaign == null || body.Campaign.Length == 0 || string.IsNullOrWhiteSpace(body.Title))
            {
                string error = "Invalid File Upload";
                return BadRequest(new { error });
            }

            Campaign campaign = new Campaign
            {
                Title = body.Title,
                Description = body.Description
            };

            try
            {
                List<Challenge> challenges = new List<Challenge>();

                using Stream stream = body.Campaign.OpenReadStream();
                using ZipArchive archive = new ZipArchive(stream);

                string coverName = archive.Entries.First(e => e.Name.StartsWith("cover")).Name;

                ZipArchiveEntry infoEntry = archive.GetEntry("info.json");
                ZipArchiveEntry coverEntry = archive.GetEntry(coverName);

                using Stream infoStream = infoEntry.Open();
                using StreamReader infoText = new StreamReader(infoStream);

                string infoJson = await infoText.ReadToEndAsync();
                CampaignInfo campaignInfo = JsonSerializer.Deserialize<CampaignInfo>(infoJson);

                using SHA256 sha256 = SHA256.Create();
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(infoJson));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2"));
                }

                campaign.Id = Guid.NewGuid();
                campaign.Hash = builder.ToString().ToLower();
                campaign.UploaderId = user.Id;
                campaign.Uploaded = DateTime.UtcNow;
                campaign.Stats = new CampaignStats
                {
                    DownloadCount = 0,
                    Votes = new List<Vote>()
                };

                if (await _campaignSaberContext.Campaigns.AnyAsync(c => c.Hash == campaign.Hash))
                {
                    throw new Exception("Map already uploaded");
                }

                int nodeCount = campaignInfo.MapPositions.Length;
                for (int i = 0; i < nodeCount; i++)
                {
                    ZipArchiveEntry node = archive.GetEntry(i + ".json");
                    if (node != null)
                    {
                        using Stream nodeStream = node.Open();
                        using StreamReader nodeText = new StreamReader(nodeStream);
                        string nodeJson = await nodeText.ReadToEndAsync();
                        Challenge challenge = JsonSerializer.Deserialize<Challenge>(nodeJson);
                        challenge.NodeName = campaignInfo.MapPositions[i].NumberPortion.ToString() + campaignInfo.MapPositions[i].LetterPortion;
                        challenges.Add(challenge);
                    }
                    else
                    {
                        // The number of nodes are not synchronized.
                        throw new Exception("Invalid node count");
                    }
                }
                if (nodeCount != challenges.Count)
                    throw new Exception("Invalid node count");

                string saveFolder = Path.Combine("files", campaign.Id.ToString());
                using Stream coverStream = coverEntry.Open();
                string fileExtension = Path.GetExtension(coverName);
                string coverLocation = Path.Combine(saveFolder, campaign.Hash + fileExtension);
                bool imageValid = Utilities.VerifyImageFileExtension(coverStream, fileExtension);
                if (!imageValid)
                    throw new Exception("Invalid Image File. Must be a png, jpg, or gif");
                Directory.CreateDirectory(saveFolder);
               
                string zipLocation = Path.Combine(saveFolder, campaign.Hash + ".zip");
                using FileStream zipFileStream = System.IO.File.Create(zipLocation);
                stream.Seek(0, SeekOrigin.Begin);
                await stream.CopyToAsync(zipFileStream);

                using FileStream coverFileStream = System.IO.File.Create(coverLocation);
                //coverStream.Seek(0, SeekOrigin.Begin);
                using Stream newCoverStream = coverEntry.Open();
                await newCoverStream.CopyToAsync(coverFileStream);
                string htmlColor = ColorTranslator.ToHtml(Color.FromArgb((int)(campaignInfo.LightColor.R * 255), (int)(campaignInfo.LightColor.G * 255), (int)(campaignInfo.LightColor.B * 255)));
                campaign.Metadata = new CampaignMetadata
                {
                    Challenges = challenges.ToArray(),
                    Color = htmlColor
                };
                campaign.CoverURL = "/" + coverLocation.Replace("\\", "/");
                campaign.DownloadURL = "/api/download/" + campaign.Id;
                campaign.DirectDownloadURL = "/" + zipLocation.Replace("\\", "/"); ;

                campaign = (await _campaignSaberContext.Campaigns.AddAsync(campaign)).Entity;
                await _campaignSaberContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string error = e.Message;
                return BadRequest(new { error });
            }
            return Ok(campaign);
        }
    }
}