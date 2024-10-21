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
    public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorites(int userId)
    {
        var favorites = await _kajblogDbContext.Favorites
            .Where(f => f.UserId == userId)
            .ToListAsync();
        return Ok(favorites);
    }

    [HttpPost]
    public async Task<ActionResult<Favorite>> AddFavorite(Favorite favorite)
    {
        _kajblogDbContext.Favorites.Add(favorite);
        await _kajblogDbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFavorites), new { userId = favorite.UserId }, favorite);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveFavorite(int id)
    {
        var favorite = await _kajblogDbContext.Favorites.FindAsync(id);
        if (favorite == null)
        {
            return NotFound();
        }

        _kajblogDbContext.Favorites.Remove(favorite);
        await _kajblogDbContext.SaveChangesAsync();
        return NoContent();
    }
    private bool FavoriteExists(int id)
    {
        return _kajblogDbContext.Favorites.Any(e => e.Id == id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFavorite(int id, Favorite favorite)
    {
        
        if (id != favorite.Id)
        {
            return BadRequest("Favorite ID mismatch.");
        }

       
        var existingFavorite = await _kajblogDbContext.Favorites.FindAsync(id);
        if (existingFavorite == null)
        {
            return NotFound("Favorite not found.");
        }

      
        existingFavorite.UserId = favorite.UserId; 
        existingFavorite.BlogId = favorite.BlogId; 

        
        _kajblogDbContext.Entry(existingFavorite).State = EntityState.Modified;

        var result = await _kajblogDbContext.SaveChangesAsync();

        
        if (result == 0)
        {
            return NotFound("Favorite not found.");
        }

        return NoContent();
    }

    private bool FavoriteDisplays(int id)
    {
        return _kajblogDbContext.Favorites.Any(e => e.Id == id);
    }

}