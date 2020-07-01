namespace CampaignSaber.Models.Settings
{
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }

    public interface IJWTSettings
    {
        string Key { get; set; }
        string Issuer { get; set; }
    }
}