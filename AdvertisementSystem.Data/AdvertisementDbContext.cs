namespace AdvertisementSystem.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class AdvertisementDbContext : IdentityDbContext<User>
    {
        public AdvertisementDbContext(DbContextOptions<AdvertisementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ad> Ads { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<AdTag>()
                .HasKey(at => new { at.AdId, at.TagId });

            builder
                .Entity<AdTag>()
                .HasOne(at => at.Ad)
                .WithMany(a => a.Tags)
                .HasForeignKey(at => at.AdId);

            builder
                .Entity<AdTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.Ads)
                .HasForeignKey(at => at.TagId);

            builder
                .Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId);

            builder
                .Entity<Comment>()
                .HasOne(c => c.Ad)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AdId);

            builder
                .Entity<Ad>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Ads)
                .HasForeignKey(a => a.AuthorId);

            builder
                .Entity<Ad>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CategoryId);

            base.OnModelCreating(builder);
        }
    }
}
