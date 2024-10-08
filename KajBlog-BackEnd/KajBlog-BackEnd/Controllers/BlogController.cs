using KajBlog_BackEnd.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using KajBlog_BackEnd.Models;
using AutoMapper;
using KajBlog_BackEnd.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace KajBlog_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ApiBaseController
    {
        private KajBlogDbContext _kajblogDbContext;
        private IMapper _mapper;

        public BlogController(KajBlogDbContext kajblogDbContext, IMapper mapper)
        {
            _kajblogDbContext = kajblogDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogs()

        {
            var blogs = await _kajblogDbContext.Blogs.ToListAsync();
            return Ok(_mapper.Map<List<BlogDto>>(blogs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlogById(int id)
        {
            var blog = await _kajblogDbContext.Blogs.FindAsync(id);

            if (blog == null)
            {

                return NotFound(); 
            }

            return Ok(_mapper.Map<BlogDto>(blog)); 
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Blog>> CreateBlog([FromBody] CreateBlogDto blogDto)
        {
            if (blogDto == null)
            {
                return BadRequest();
            }

            var blog = _mapper.Map<Blog>(blogDto);

            blog.CreatedOn = DateTime.Now;
            blog.CreatedBy = GetCurrentUserID();
            blog.UserId = GetCurrentUserID();

            _kajblogDbContext.Blogs.Add(blog);
            await _kajblogDbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlogById), new { id = blog.BlogId }, _mapper.Map<BlogDto>(blog));
        }

        [HttpDelete("{id}")]
        [Authorize]
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

//Need a Put at some point