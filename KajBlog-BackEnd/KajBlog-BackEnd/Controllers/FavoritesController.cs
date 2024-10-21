using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KajBlog_BackEnd.Data;
using KajBlog_BackEnd.Models;
using KajBlog_BackEnd.Models.DTO;
using Microsoft.AspNetCore.Authorization;


namespace KajBlog_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize]
public class FavoritesController : ApiBaseController
{
    private readonly KajBlogDbContext _kajblogDbContext;

    public FavoritesController(KajBlogDbContext kajblogDbContext)
    {
        _kajblogDbContext = kajblogDbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BlogDto>>> GetFavorites()
    {
        var userId = GetCurrentUserID();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var favorites = await _kajblogDbContext.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Blog)
            .Select(f => new BlogDto
            {
                BlogId = f.BlogId,
                UserId = f.Blog.UserId,
                Category = f.Blog.Category,
                SubjectLine = f.Blog.SubjectLine,
                BlogBody = f.Blog.BlogBody,
                GiphyPull = f.Blog.GiphyPull,
                CreatedOn = f.Blog.CreatedOn
            })
            .ToListAsync();

        return Ok(favorites);
    }

    [HttpPost("blog/{blogId}")]
    public async Task<ActionResult<Favorite>> AddFavorite(int blogId)
    {
        var userId = GetCurrentUserID();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var blog = await _kajblogDbContext.Blogs.FindAsync(blogId);
        if (blog == null)
        {
            return NotFound($"Blog with ID {blogId} does not exist.");
        }

        var existingFavorite = await _kajblogDbContext.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.BlogId == blogId);

        if (existingFavorite != null)
        {
            return Conflict("This blog is already favorited.");
        }

        var favorite = new Favorite
        {
            BlogId = blogId,
            UserId = userId,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = userId
        };

        _kajblogDbContext.Favorites.Add(favorite);
        await _kajblogDbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFavorite(int id)
    {
        var userId = GetCurrentUserID();
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var favorite = await _kajblogDbContext.Favorites.FindAsync(id);
        if (favorite == null || favorite.UserId != userId)
        {
            return NotFound();
        }

        _kajblogDbContext.Favorites.Remove(favorite);
        await _kajblogDbContext.SaveChangesAsync();

        return NoContent();
    }
}