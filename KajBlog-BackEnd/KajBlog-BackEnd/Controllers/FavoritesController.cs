using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KajBlog_BackEnd.Data;

using KajBlog_BackEnd.Models;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http.HttpResults;


namespace KajBlog_BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private KajBlogDbContext _kajblogDbContext;
    public FavoritesController(KajBlogDbContext kajblogDbContext)
    {
        _kajblogDbContext = kajblogDbContext;
    }

    [HttpGet("{userId}")] // Get: api/Favorites/User/5
    public async Task<IActionResult> GetFavoritesByUserId(int userId)
    {
        IQueryable<Favorite> userFavorites = _kajblogDbContext.Favorites.Where(x => x.UserId == userId);

        var result = from favorites in userFavorites
                     join blog_ in _kajblogDbContext.Blogs on favorites.BlogId equals blog_.BlogId into FavoritedBlogs
                     from m in FavoritedBlogs.DefaultIfEmpty()
                     select new
                     {
                         BlogId = favorites.BlogId,
                         Catagory = m.Category,
                         SubjectLine = m.SubjectLine,
                         BlogBody = m.BlogBody,
                         TimeStamp = m.TimeStamp,
                         GiphyPull = m.GiphyPull
                     };

        return Ok(await result.ToListAsync());
    }


    [HttpGet]// Get: api/Favorites
    public async Task<ActionResult<IEnumerable<Favorite>>> GetFavorites()
    {
        return await _kajblogDbContext.Favorites.ToListAsync();
    }


    [HttpPost]
    public async Task<IActionResult> CreateFavorite([FromBody] Favorite favorite)
    {
        Favorite newFavorite = new Favorite();

        newFavorite.Id = favorite.Id;
        newFavorite.UserId = favorite.UserId;
        newFavorite.BlogId = favorite.BlogId;

        _kajblogDbContext.Add(newFavorite);

        await _kajblogDbContext.SaveChangesAsync();

        return Ok(newFavorite);
    }


    [HttpPut("{id}")] // PUT: api/Favorites/5
    public async Task<IActionResult> PutFavorite(int id, Favorite favorite)
    {
        if (id != favorite.Id)
        {
            return BadRequest();
        }

        _kajblogDbContext.Entry(favorite).State = EntityState.Modified;

        try
        {
            await _kajblogDbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!FavoriteExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")] // DELETE: api/Favorites/5
    public async Task<IActionResult> DeleteFavorite(int id)
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


    [HttpDelete("User/{userId}/Blog/{blogId}")] // DELETE: api/Favorites/User/5/Blog/10
    public async Task<IActionResult> DeleteFavoriteByUserAndBlog(int userId, int blogId)
    {
        var favorite = await _kajblogDbContext.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.BlogId == blogId);

        if (favorite == null)
        {
            return NotFound();
        }

        _kajblogDbContext.Favorites .Remove(favorite);
        await _kajblogDbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool FavoriteExists(int id)
    {
        return _kajblogDbContext.Favorites.Any(e => e.Id == id);
    }
}
