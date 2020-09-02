namespace CampaignSaber.Models.Campaigns
{
    public class GameplayModifiers
    {
        public enum EnabledObstacleType
        {
            All,
            FullHeightOnly,
            NoObstacles
        }

        public enum EnergyType
        {
            Bar,
            Battery
        }
        public enum SongSpeed
        {
            Normal,
            Faster,
            Slower
        }
    }
}