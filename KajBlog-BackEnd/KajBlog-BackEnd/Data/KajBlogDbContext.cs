using Microsoft.EntityFrameworkCore;

namespace KajBlog_BackEnd.Data;

public class KajBlogDbContext : DbContext
{
    public KajBlogDbContext(DbContextOptions<KajBlogDbContext> options) : base(options) { }
}
