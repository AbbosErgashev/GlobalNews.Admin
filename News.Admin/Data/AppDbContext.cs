using Microsoft.EntityFrameworkCore;
using News.Admin.Models;

namespace News.Admin.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {   }

    public required DbSet<NewsItem> NewsItems { get; set; }
}
