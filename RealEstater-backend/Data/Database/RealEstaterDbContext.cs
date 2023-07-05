using Microsoft.EntityFrameworkCore;
using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Data.Database
{
    public class RealEstaterDbContext : DbContext
    {
        public RealEstaterDbContext(DbContextOptions<RealEstaterDbContext> options) : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<FeatureModel> Features { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<LandholdingModel> Landholdings { get; set; }
        public DbSet<PriceHistoryModel> PriceHistory { get; set; }
        public DbSet<LandholdingPictureModel> LandholdingPictures { get; set; }
        public DbSet<LandholdingFeatureModel> LandholdingsFeatures { get; set; }
        public DbSet<ConstructionStageModel> ConstructionStages { get; set; }
        public DbSet<ConstructionTypeModel> ConstructionTypes { get; set; }
        public DbSet<LandholdingTypeModel> LandholdingTypes { get; set; }
        public DbSet<ConversationModel> Conversations { get; set; }
        public DbSet<ConversationReplyModel> ConversationReplies { get; set; }
        public DbSet<ConversationStatusModel> ConversationStatuses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasMany(x => x.Landholdings)
                .WithOne(x => x.User);

            modelBuilder.Entity<LandholdingModel>()
                .HasMany(x => x.Pictures)
                .WithOne(x => x.Landholding);

            modelBuilder.Entity<CityModel>()
                .HasMany(x => x.Addresses)
                .WithOne(x => x.City);

            modelBuilder.Entity<LandholdingFeatureModel>()
                .HasOne(x => x.Feature)
                .WithMany(x => x.LandholdingFeatures)
                .HasForeignKey(x => x.FeatureId);

            modelBuilder.Entity<LandholdingFeatureModel>()
                .HasOne(x => x.Landholding)
                .WithMany(x => x.LandholdingFeatures)
                .HasForeignKey(x => x.LandholdingId);

            modelBuilder.Entity<LandholdingModel>()
               .HasMany(x => x.HistoryPrice)
               .WithOne(x => x.Landholding);

            modelBuilder.Entity<ConversationModel>()
                .HasOne(e => e.UserOne)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ConversationModel>()
                .HasOne(e => e.UserTwo)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
