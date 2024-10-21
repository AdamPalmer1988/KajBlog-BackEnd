using KajBlog_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KajBlog_BackEnd.Data;

public class KajBlogDbContext : DbContext
{
    public KajBlogDbContext(DbContextOptions<KajBlogDbContext> options) : base(options) { }

    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<Favorite> Favorites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favorite>()
            .HasKey(f => f.Id);  

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Blog)
            .WithMany(b => b.Favorites)
            .HasForeignKey(f => f.BlogId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Blog>().HasData(
            new Blog
            {
                BlogId = 1,
                UserId = "user1",
                Category = "Tech",
                SubjectLine = "Exploring C# 10 Features",
                BlogBody = "C# 10 introduces a number of exciting features, such as global usings and file-scoped namespaces.",
                GiphyPull = "https://giphy.com/some-url",
                CreatedBy = "user1",
                CreatedOn = DateTime.UtcNow
            },
            new Blog
            {
                BlogId = 2,
                UserId = "user2",
                Category = "Travel",
                SubjectLine = "Top 10 Destinations for 2024",
                BlogBody = "If you’re looking for the next best travel spots, check out these incredible destinations for 2024.",
                GiphyPull = "https://giphy.com/some-url-2",
                CreatedBy = "user2",
                CreatedOn = DateTime.UtcNow
            },
            new Blog
            {
                BlogId = 3,
                UserId = "user3",
                Category = "Health",
                SubjectLine = "Mindfulness Techniques for Beginners",
                BlogBody = "Mindfulness can help you stay in the present moment and reduce stress. Here's how to get started.",
                GiphyPull = "https://giphy.com/some-url-3",
                CreatedBy = "user3",
                CreatedOn = DateTime.UtcNow
            }
        );

        base.OnModelCreating(modelBuilder);
    }

}
