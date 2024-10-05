using KajBlog_BackEnd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KajBlog_BackEnd.Models;

namespace KajBlog_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private KajBlogDbContext _kajblogDbContext;

        public BlogController(KajBlogDbContext kajblogDbContext)
        {
            _kajblogDbContext = kajblogDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()

        {
            var blogs = await _kajblogDbContext.Blogs.ToListAsync();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlogById(int id)
        {
            var blog = await _kajblogDbContext.Blogs.FindAsync(id);

            if (blog == null)
            {

                return NotFound(); 
            }

            return Ok(blog); 
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog([FromBody] Blog blog)
        {
            if (blog == null)
            {
                return BadRequest();
            }

            _kajblogDbContext.Blogs.Add(blog);
            await _kajblogDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlogById), new { id = blog.BlogId }, blog);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogById(int id)
        {
            var blog = await _kajblogDbContext.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }

            _kajblogDbContext.Blogs.Remove(blog);
            await _kajblogDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}