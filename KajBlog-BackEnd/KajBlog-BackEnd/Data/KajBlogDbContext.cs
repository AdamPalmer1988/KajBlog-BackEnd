using KajBlog_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KajBlog_BackEnd.Data;

public class KajBlogDbContext : DbContext
{
    public KajBlogDbContext(DbContextOptions<KajBlogDbContext> options) : base(options) { }

    public virtual DbSet<Blog> Blogs { get; set; }
    public virtual DbSet<Favorite> Favorites { get; set; }
    public virtual DbSet<User> Users { get; set; }
}
