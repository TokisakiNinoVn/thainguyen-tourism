using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using dbConfig;
using System.Security.Claims;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/web/blog")]
    public class BlogControllerWeb : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public BlogControllerWeb(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet]
        public IActionResult GetListAllBlogs()
        {
            var blogs = new List<object>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, title, status, authorId, thumbnail, createdAt, updatedAt FROM blogs", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        blogs.Add(new
                        {
                            Id = reader["id"],
                            Title = reader["title"],
                            Status = reader["status"],
                            AuthorId = reader["authorId"],
                            Thumbnail = reader["thumbnail"],
                            CreatedAt = reader["createdAt"],
                            UpdatedAt = reader["updatedAt"]
                        });
                    }
                }
            }

            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlogDetail(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, title, content, status, authorId, thumbnail, createdAt, updatedAt FROM blogs WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var blog = new
                        {
                            Id = reader["id"],
                            Title = reader["title"],
                            Content = reader["content"],
                            Status = reader["status"],
                            AuthorId = reader["authorId"],
                            Thumbnail = reader["thumbnail"],
                            CreatedAt = reader["createdAt"],
                            UpdatedAt = reader["updatedAt"]
                        };

                        return Ok(blog);
                    }
                    else
                    {
                        return NotFound(new { message = "Blog không tồn tại." });
                    }
                }
            }
        }

        [HttpGet("reviews/{id}")]
        public IActionResult GetReviewsByBlogId(int id)
        {
            var reviews = new List<object>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, blogId, userId, rating, reviewDescription, createdAt, updatedAt FROM reviews WHERE blogId = @blogId", conn);
                cmd.Parameters.AddWithValue("@blogId", id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reviews.Add(new
                        {
                            Id = reader["id"],
                            BlogId = reader["blogId"],
                            UserId = reader["userId"],
                            Rating = reader["rating"],
                            ReviewDescription = reader["reviewDescription"],
                            CreatedAt = reader["createdAt"],
                            UpdatedAt = reader["updatedAt"]
                        });
                    }
                }
            }

            return Ok(reviews);
        }

        [HttpPost("create")]
        public IActionResult CreateBlog([FromBody] Blog blog)
        {
            var authorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (authorId == null)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            using var conn = DbConfig.GetConnection();
            conn.Open();

            // Sử dụng parameterized query để tránh SQL injection
            var cmd = new MySqlCommand("INSERT INTO blogs (title, place, content, status, authorId, thumbnail, createdAt, updatedAt) VALUES (@title, @place, @content, @status, @authorId, @thumbnail, @createdAt, @updatedAt); SELECT LAST_INSERT_ID();", conn);
            cmd.Parameters.AddWithValue("@title", blog.Title);
            cmd.Parameters.AddWithValue("@place", blog.Place);
            cmd.Parameters.AddWithValue("@content", blog.Content);
            cmd.Parameters.AddWithValue("@status", blog.Status);
            cmd.Parameters.AddWithValue("@authorId", authorId); // Sử dụng authorId từ token, không phải từ body
            cmd.Parameters.AddWithValue("@thumbnail", blog.Thumbnail.GetValueOrDefault()); // Xử lý thumbnail null hoặc rỗng
            cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow);

            // Thực thi truy vấn và lấy ID vừa được tạo
            long lastInsertedId = Convert.ToInt64(cmd.ExecuteScalar());

            if (lastInsertedId > 0)
            {
                return Ok(new
                {
                    id = lastInsertedId,
                });
            }
            else
            {
                return BadRequest(new { message = "Failed to create blog." });
            }
        }

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadSingle([FromForm] UploadThumbnailRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest(new { message = "File is empty" });

            if (request.Type < 1 || request.Type > 4)
                return BadRequest(new { message = "Invalid media type" });

            if (string.IsNullOrEmpty(request.ImageFor) || (request.ImageFor != "thumbnail" && request.ImageFor != "media"))
                return BadRequest(new { message = "ImageFor must be 'thumbnail' or 'media'" });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var media = new BlogMedia
            {
                BlogId = request.BlogId,
                MediaUrl = $"/uploads/{fileName}",
                MediaType = request.Type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ImageFor = request.ImageFor
            };

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("INSERT INTO blog_media (blogId, mediaUrl, mediaType, createdAt, updatedAt, imageFor) VALUES (@blogId, @mediaUrl, @mediaType, @createdAt, @updatedAt, @imageFor)", conn);
                cmd.Parameters.AddWithValue("@blogId", media.BlogId);
                cmd.Parameters.AddWithValue("@mediaUrl", media.MediaUrl);
                cmd.Parameters.AddWithValue("@mediaType", media.MediaType);
                cmd.Parameters.AddWithValue("@createdAt", media.CreatedAt);
                cmd.Parameters.AddWithValue("@updatedAt", media.UpdatedAt);
                cmd.Parameters.AddWithValue("@imageFor", media.ImageFor);

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    media.Id = (int)cmd.LastInsertedId;

                    // If the upload is for a thumbnail, update the blog's thumbnail field
                    if (media.ImageFor == "thumbnail")
                    {
                        var updateCmd = new MySqlCommand("UPDATE blogs SET thumbnail = @thumbnailId WHERE id = @blogId", conn);
                        updateCmd.Parameters.AddWithValue("@thumbnailId", media.Id);
                        updateCmd.Parameters.AddWithValue("@blogId", media.BlogId);
                        updateCmd.ExecuteNonQuery();
                    }

                    return Ok(new
                    {
                        message = "File uploaded successfully.",
                        media = new
                        {
                            media.Id,
                            media.BlogId,
                            media.MediaUrl,
                            media.MediaType,
                            media.CreatedAt,
                            media.UpdatedAt,
                            media.ImageFor
                        }
                    });
                }
                else
                {
                    return BadRequest(new { message = "Failed to save media." });
                }
            }
        }

        [HttpDelete("delete-file/{id}")]
        public IActionResult DeleteFile(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();

                // Check if the media is a thumbnail
                var checkCmd = new MySqlCommand("SELECT blogId, imageFor FROM blog_media WHERE id = @id", conn);
                checkCmd.Parameters.AddWithValue("@id", id);
                int? blogId = null;
                string? imageFor = null;
                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        blogId = reader.IsDBNull(reader.GetOrdinal("blogId")) ? null : reader.GetInt32("blogId");
                        imageFor = reader.IsDBNull(reader.GetOrdinal("imageFor")) ? null : reader.GetString("imageFor");
                    }
                }

                // Delete the media
                var cmd = new MySqlCommand("DELETE FROM blog_media WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    // If it was a thumbnail, update the blog's thumbnail field to NULL
                    if (imageFor == "thumbnail" && blogId.HasValue)
                    {
                        var updateCmd = new MySqlCommand("UPDATE blogs SET thumbnail = NULL WHERE id = @blogId", conn);
                        updateCmd.Parameters.AddWithValue("@blogId", blogId.Value);
                        updateCmd.ExecuteNonQuery();
                    }

                    return Ok(new { message = "File deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "File not found." });
                }
            }
        }
    }

    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = "draft";
        public int AuthorId { get; set; }
        public int? Thumbnail { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UploadThumbnailRequest
    {
        public int BlogId { get; set; }
        public int Type { get; set; } // 1: image, 2: video, etc.
        public string ImageFor { get; set; } = string.Empty; // thumbnail, media
        public required IFormFile File { get; set; }
    }

    public class BlogMedia
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public int MediaType { get; set; } // 1: image, 2: video
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ImageFor { get; set; } = string.Empty; // thumbnail, media
    }
}