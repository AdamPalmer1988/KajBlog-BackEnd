using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KajBlog_BackEnd.Data;

using KajBlog_BackEnd.Models;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using KajBlog_BackEnd.Models.DTO;


namespace KajBlog_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ApiBaseController
{
    private KajBlogDbContext _kajblogDbContext;
    public FavoritesController(KajBlogDbContext kajblogDbContext)
    {
        _kajblogDbContext = kajblogDbContext;
    }

    [HttpGet("{userId}")] 
    public async Task<IActionResult> GetFavoritesByUserId(int userId)
    {
        IQueryable<Favorite> userFavorites = _kajblogDbContext.Favorites.Where(x => x.UserId == userId);

        var result = new List<object>();

        foreach (var favorite in await userFavorites.ToListAsync())
        {
            var blogDto = await _kajblogDbContext.Blogs.FindAsync(favorite.BlogId);

            if (blogDto != null)
            {
                result.Add(new
                {
                    BlogId = favorite.BlogId,
                    Category = blogDto.Category,
                    SubjectLine = blogDto.SubjectLine,
                    BlogBody = blogDto.BlogBody,
                    GiphyPull = blogDto.GiphyPull
                });
            }
        }

        return Ok(result);
    }
    [HttpGet]
    public ActionResult<IEnumerable<FavoriteDto>> GetFavorites()
    {
        var favorites = _kajblogDbContext.Favorites
            .Select(f => new FavoriteDto
            {
                Id = f.Id,
                UserId = f.UserId,
                BlogId = f.BlogId
            }).ToList();

        return Ok(favorites);
    }
    [HttpPost]
    public async Task<IActionResult> CreateFavorite([FromBody] FavoriteDto favoriteDto)
    {
        if (favoriteDto == null)
        {
            return BadRequest("Favorite data is null.");
        }

        var favorite = new Favorite
        {
            UserId = favoriteDto.UserId,
            BlogId = favoriteDto.BlogId
        };

        await _kajblogDbContext.Favorites.AddAsync(favorite);
        await _kajblogDbContext.SaveChangesAsync();

        return CreatedAtRoute("GetFavoriteById", new { id = favorite.Id }, favorite);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFavorite(int id, [FromBody] FavoriteDto favoriteDto)
    {
        if (favoriteDto == null)
        {
            return BadRequest("Favorite data is null.");
        }

        var existingFavorite = await _kajblogDbContext.Favorites.FindAsync(id);
        if (existingFavorite == null)
        {
            return NotFound(); 
        }

        existingFavorite.UserId = favoriteDto.UserId;
        existingFavorite.BlogId = favoriteDto.BlogId;

        await _kajblogDbContext.SaveChangesAsync();

        return Ok(existingFavorite);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFavorite(int id)
    {
        
        var existingFavorite = await _kajblogDbContext.Favorites.FindAsync(id);
        if (existingFavorite == null)
        {
            return NotFound(); 
        }

        
        _kajblogDbContext.Favorites.Remove(existingFavorite);
        await _kajblogDbContext.SaveChangesAsync(); 

        return NoContent();
    }

}