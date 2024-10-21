using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KajBlog_BackEnd.Data;

using KajBlog_BackEnd.Models;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using KajBlog_BackEnd.Models.DTO;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Authorization;


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


    [HttpGet("{blogId}")]
    public async Task<IActionResult> GetFavoriteByBlogId(int blogId)
    {
        IQueryable<Favorite> userFavorites = _kajblogDbContext.Favorites.Where(x => x.BlogId == blogId);

        var result = from favorites in userFavorites
                     join blog_ in _kajblogDbContext.Blogs on favorites.BlogId equals blog_.BlogId into FavoritedBlogs
                     from m in FavoritedBlogs.DefaultIfEmpty()
                     select new
                     {
                         BlogId = favorites.BlogId,
                         UserId = m.UserId,
                         Category = m.Category,
                         SubjectLine = m.SubjectLine,
                         BlogBody = m.BlogBody,
                         GiphyPull = m.GiphyPull,
                     };
        return Ok(await result.ToListAsync());

    }

    [HttpPost("blog/{blogId}")]
    [Authorize]
    public async Task<IActionResult> CreateFavorite(int blogId)
        {
         Favorite newFavorite = new Favorite();

        newFavorite.BlogId = blogId;
        newFavorite.UserId = GetCurrentUserID();
        newFavorite.CreatedBy = GetCurrentUserID();
        newFavorite.CreatedOn = DateTime.UtcNow;


         _kajblogDbContext.Add(newFavorite);

         await _kajblogDbContext.SaveChangesAsync();

         return Ok(newFavorite);
        }


    [HttpDelete("{blogId}")]
    public async Task<IActionResult> DeleteFavorite(int blogId)
    {
        var favoriteEntity = await _kajblogDbContext.Favorites.FirstOrDefaultAsync(x => x.BlogId == blogId);

        if(favoriteEntity == null)
        {
            return NotFound();
        }

        _kajblogDbContext.Favorites.Remove(favoriteEntity);
        await _kajblogDbContext.SaveChangesAsync(true);

        return NoContent();
    }

   
    private bool FavoriteExists(int id)
    {
        return _kajblogDbContext.Favorites.Any(e => e.Id == id);
    }

    private bool FavoriteDisplays(int id)
    {
        return _kajblogDbContext.Favorites.Any(e => e.Id == id);
    }

}