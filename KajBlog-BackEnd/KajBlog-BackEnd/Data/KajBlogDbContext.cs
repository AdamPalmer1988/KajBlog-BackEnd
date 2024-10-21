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
        // Configure the relationship between Blog and Favorite with cascade delete
        modelBuilder.Entity<Favorite>() .HasKey(f => f.Id); 
        // Primary key for Favorite
        modelBuilder.Entity<Favorite>() .HasOne(f => f.Blog) .WithMany(b => b.Favorites) .HasForeignKey(f => f.BlogId) .OnDelete(DeleteBehavior.Cascade); 
        // Enable cascade
        base.OnModelCreating(modelBuilder); }
    }
