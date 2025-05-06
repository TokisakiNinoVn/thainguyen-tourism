//  file: Controllers/admin/blog/BlogController.cs

using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
// [Authorize(Roles = "admin")]
[Route("api/admin/blog")]
public class BlogController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public BlogController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetBlogs()
    {
        var blogs = await _context.Blogs.ToListAsync();
        return Ok(blogs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBlog(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return NotFound("Blog not found");
        return Ok(blog);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBlog([FromBody] Blog blog)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (blog == null) return BadRequest("Invalid blog data");

        blog.AuthorId = int.Parse(userId ?? "0");
        blog.CreatedAt = DateTime.UtcNow;
        blog.UpdatedAt = DateTime.UtcNow;
        blog.Thumbnail = 0;
        blog.Place = string.Empty;
        blog.Status = "0";
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBlog), new { id = blog.Id }, blog);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBlog(int id, [FromBody] Blog blog)
    {
        if (id != blog.Id) return BadRequest("Blog ID mismatch");

        _context.Entry(blog).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return NotFound("Blog not found");

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Phê duyêt bài viết
    [HttpPut("approve/{id}")]
    public async Task<IActionResult> ApproveBlog(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null) return NotFound("Blog not found");

        blog.Status = "approved";
        _context.Entry(blog).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

public class BlogMedia
{
    public int Id { get; set; }
    public int BlogId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public int MediaType { get; set; } // 1: image, 2: video,
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}