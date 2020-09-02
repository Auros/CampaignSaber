using CampaignSaber.Models.Campaigns;
using CampaignSaber.Models.Settings;
using Microsoft.EntityFrameworkCore;

namespace CampaignSaber.Models
{
    public class CampaignSaberContext : DbContext
    {
        private readonly IDatabaseSettings _databaseSettings;

        public DbSet<User> Users { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        public CampaignSaberContext(IDatabaseSettings databaseSettings)
        {
            _databaseSettings = databaseSettings;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_databaseSettings.ConnectionString);
            optionsBuilder.UseSnakeCaseNamingConvention();
        }
    }
}