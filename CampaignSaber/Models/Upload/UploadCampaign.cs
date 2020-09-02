using Microsoft.AspNetCore.Http;

namespace CampaignSaber.Models.Upload
{
    public class UploadCampaign
    {
        public string Title { get; set; }

        public IFormFile Campaign { get; set; }
        
        public string Description { get; set; }
    }
}