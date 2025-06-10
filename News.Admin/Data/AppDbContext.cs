using Microsoft.EntityFrameworkCore;
using News.Admin.Models;

namespace News.Admin.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public required DbSet<NewsItem> NewsItems { get; set; }
    public required DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<NewsItem>()
        //    .HasOne(n => n.Category)
        //    .WithMany(c => c.NewsItems)
        //    .HasForeignKey(n => n.CategoryId)
        //    .OnDelete(DeleteBehavior.SetNull);
    }
}
