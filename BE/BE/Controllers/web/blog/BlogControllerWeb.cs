using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Security.Claims;
using dbConfig;
using System.Data;

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

        [HttpGet("list")]
        public IActionResult GetListAllBlogs()
        {
            var blogs = new List<Dictionary<string, object>>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, title, place, content, authorId, thumbnail, createdAt, updatedAt, category FROM blogs WHERE statusBlog = 'approved'", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var blog = new Dictionary<string, object>
                        {
                            { "Id", reader.GetInt32("id") },
                            { "Title", reader.GetString("title") },
                            { "Place", reader.GetString("place") },
                            { "Content", reader.GetString("content") },
                            { "AuthorId", reader.GetInt32("authorId") },
                            { "Thumbnail", reader.GetInt32("thumbnail") },
                            { "CreatedAt", reader.GetDateTime("createdAt") },
                            { "UpdatedAt", reader.GetDateTime("updatedAt") },
                            { "Category", reader.GetInt32("category") }
                        };
                        blogs.Add(blog);
                    }
                }

                // Close the reader before executing another command
                foreach (var blog in blogs)
                {
                    var mediaList = new List<object>();
                    using (var mediaCmd = new MySqlCommand("SELECT id, blogId, mediaUrl, mediaType, imageFor FROM blogmedia WHERE blogId = @blogId AND imageFor = 'thumbnail'", conn))
                    {
                        mediaCmd.Parameters.AddWithValue("@blogId", blog["Id"]);
                        using (var mediaReader = mediaCmd.ExecuteReader())
                        {
                            while (mediaReader.Read())
                            {
                                mediaList.Add(new
                                {
                                    Id = mediaReader.GetInt32("id"),
                                    BlogId = mediaReader.GetInt32("blogId"),
                                    MediaUrl = mediaReader.GetString("mediaUrl"),
                                    MediaType = mediaReader.GetInt32("mediaType"),
                                    ImageFor = mediaReader.GetString("imageFor"),
                                });
                            }
                        }
                    }
                    blog["MediaList"] = mediaList;
                }
            }

            return Ok(blogs);
        }

        [HttpGet("details/{id}")]
        public IActionResult GetBlogDetail(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, title, content, place, statusBlog, authorId, thumbnail, createdAt, updatedAt FROM blogs WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    var blog = new
                    {
                        Id = reader.GetInt32("id"),
                        Title = reader.GetString("title"),
                        Content = reader.GetString("content"),
                        Status = reader.GetString("statusBlog"),
                        AuthorId = reader.GetInt32("authorId"),
                        Place = reader.GetString("place"),
                        Thumbnail = reader.GetInt32("thumbnail"),
                        CreatedAt = reader.GetDateTime("createdAt"),
                        UpdatedAt = reader.GetDateTime("updatedAt")
                    };

                    var blogData = blog;
                    reader.Close();

                    // Lấy thông tin tác giả
                    string? displayName = null;
                    string? photoUrl = null;

                    using (var userCmd = new MySqlCommand("SELECT displayName, photoUrl FROM users WHERE id = @userId", conn))
                    {
                        userCmd.Parameters.AddWithValue("@userId", blogData.AuthorId);
                        using (var userReader = userCmd.ExecuteReader())
                        {
                            if (userReader.Read())
                            {
                                displayName = userReader.IsDBNull("displayName") ? null : userReader.GetString("displayName");
                                photoUrl = userReader.IsDBNull("photoUrl") ? null : userReader.GetString("photoUrl");
                            }
                        }
                    }

                    // Lấy danh sách media
                    var mediaList = new List<object>();
                    using (var mediaCmd = new MySqlCommand("SELECT id, blogId, mediaUrl, mediaType, imageFor FROM blogmedia WHERE blogId = @blogId", conn))
                    {
                        mediaCmd.Parameters.AddWithValue("@blogId", id);
                        using (var mediaReader = mediaCmd.ExecuteReader())
                        {
                            while (mediaReader.Read())
                            {
                                mediaList.Add(new
                                {
                                    Id = mediaReader.GetInt32("id"),
                                    BlogId = mediaReader.GetInt32("blogId"),
                                    MediaUrl = mediaReader.GetString("mediaUrl"),
                                    MediaType = mediaReader.GetInt32("mediaType"),
                                    ImageFor = mediaReader.GetString("imageFor"),
                                });
                            }
                        }
                    }

                    var result = new
                    {
                        blogData.Id,
                        blogData.Title,
                        blogData.Content,
                        blogData.Status,
                        blogData.AuthorId,
                        AuthorName = displayName,
                        AuthorPhoto = photoUrl,
                        blogData.Place,
                        blogData.Thumbnail,
                        blogData.CreatedAt,
                        blogData.UpdatedAt,
                        MediaList = mediaList
                    };

                    return Ok(result);
                }
                return NotFound(new { message = "Blog không tồn tại." });
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
                            Id = reader.GetInt32("id"),
                            BlogId = reader.GetInt32("blogId"),
                            UserId = reader.GetInt32("userId"),
                            Rating = reader.GetInt32("rating"),
                            ReviewDescription = reader.GetString("reviewDescription"),
                            CreatedAt = reader.GetDateTime("createdAt"),
                            UpdatedAt = reader.GetDateTime("updatedAt")
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
            if (string.IsNullOrEmpty(authorId))
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            // log ra dữ liệu đầu vào
            Console.WriteLine($"Title: {blog.Title}, Place: {blog.Place}, Content: {blog.Content}, Status: {blog.Status}, AuthorId: {authorId}, Category: {blog.Category}");

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO blogs (title, place, content, statusBlog, authorId, thumbnail, createdAt, updatedAt, category) " +
                    "VALUES (@title, @place, @content, @statusBlog, @authorId, @thumbnail, @createdAt, @updatedAt, @categoryId); " +
                    "SELECT LAST_INSERT_ID();", conn);

                cmd.Parameters.AddWithValue("@title", blog.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("@place", blog.Place ?? string.Empty);
                cmd.Parameters.AddWithValue("@content", blog.Content ?? string.Empty);
                cmd.Parameters.AddWithValue("@statusBlog", blog.Status ?? "draft");
                cmd.Parameters.AddWithValue("@authorId", int.Parse(authorId));
                cmd.Parameters.AddWithValue("@thumbnail", blog.Thumbnail.HasValue ? blog.Thumbnail : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@categoryId", blog.Category);

                var lastInsertedId = Convert.ToInt64(cmd.ExecuteScalar());

                if (lastInsertedId > 0)
                {
                    return Ok(new { id = lastInsertedId });
                }
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
            Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO blogmedia (blogId, mediaUrl, mediaType, createdAt, updatedAt, imageFor) " +
                    "VALUES (@blogId, @mediaUrl, @mediaType, @createdAt, @updatedAt, @imageFor); " +
                    "SELECT LAST_INSERT_ID();", conn);

                cmd.Parameters.AddWithValue("@blogId", request.BlogId);
                cmd.Parameters.AddWithValue("@mediaUrl", $"/uploads/{fileName}");
                cmd.Parameters.AddWithValue("@mediaType", request.Type);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@imageFor", request.ImageFor);

                var mediaId = Convert.ToInt32(cmd.ExecuteScalar());

                if (mediaId > 0)
                {
                    if (request.ImageFor == "thumbnail")
                    {
                        var updateCmd = new MySqlCommand(
                            "UPDATE blogs SET thumbnail = @mediaId WHERE id = @blogId", conn);
                        updateCmd.Parameters.AddWithValue("@mediaId", mediaId);
                        updateCmd.Parameters.AddWithValue("@blogId", request.BlogId);
                        updateCmd.ExecuteNonQuery();
                    }

                    return Ok(new
                    {
                        message = "File uploaded successfully.",
                        media = new
                        {
                            Id = mediaId,
                            BlogId = request.BlogId,
                            MediaUrl = $"/uploads/{fileName}",
                            MediaType = request.Type,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            ImageFor = request.ImageFor
                        }
                    });
                }
                return BadRequest(new { message = "Failed to save media." });
            }
        }

        [HttpDelete("delete-file/{id}")]
        public IActionResult DeleteFile(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var checkCmd = new MySqlCommand("SELECT blogId, imageFor, mediaUrl FROM blogmedia WHERE id = @id", conn);
                checkCmd.Parameters.AddWithValue("@id", id);

                int? blogId = null;
                string? imageFor = null;
                string? mediaUrl = null;

                using (var reader = checkCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        blogId = reader.IsDBNull(reader.GetOrdinal("blogId")) ? null : reader.GetInt32("blogId");
                        imageFor = reader.IsDBNull(reader.GetOrdinal("imageFor")) ? null : reader.GetString("imageFor");
                        mediaUrl = reader.IsDBNull(reader.GetOrdinal("mediaUrl")) ? null : reader.GetString("mediaUrl");
                    }
                    else
                    {
                        return NotFound(new { message = "File not found." });
                    }
                }

                var cmd = new MySqlCommand("DELETE FROM blogmedia WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    if (imageFor == "thumbnail" && blogId.HasValue)
                    {
                        var updateCmd = new MySqlCommand(
                            "UPDATE blogs SET thumbnail = NULL WHERE id = @blogId", conn);
                        updateCmd.Parameters.AddWithValue("@blogId", blogId.Value);
                        updateCmd.ExecuteNonQuery();
                    }

                    // Delete physical file
                    if (!string.IsNullOrEmpty(mediaUrl))
                    {
                        var filePath = Path.Combine(_env.WebRootPath, mediaUrl.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    return Ok(new { message = "File deleted successfully." });
                }
                return NotFound(new { message = "File not found." });
            }
        }

        // Update blog
        [HttpPut("update/{id}")]
        public IActionResult UpdateBlog(int id, [FromBody] Blog blog)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "UPDATE blogs SET title = @title, place = @place, content = @content, updatedAt = @updatedAt WHERE id = @id", conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@title", blog.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("@place", blog.Place ?? string.Empty);
                cmd.Parameters.AddWithValue("@content", blog.Content ?? string.Empty);
                // cmd.Parameters.AddWithValue("@statusBlog", blog.Status ?? "draft");
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow);

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return Ok(new { message = "Blog updated successfully." });
                }
                return NotFound(new { message = "Blog not found." });
            }
        }

        // Tạo bình luận
        [HttpPost("create-comment")]
        public IActionResult CreateReview([FromBody] CommentBlog commentblog)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO blogcomments (blogId, userId, commentContent, createdAt, updatedAt) " +
                    "VALUES (@blogId, @userId, @commentContent,  @createdAt, @updatedAt);", conn);

                cmd.Parameters.AddWithValue("@blogId", commentblog.BlogId);
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@updatedAt", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("@commentContent", commentblog.CommentContent ?? string.Empty);

                var result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    return Ok(new { message = "Review created successfully." });
                }
                return BadRequest(new { message = "Failed to create review." });
            }
        }


        // Lấy danh sách bình luận theo blogId
        [HttpGet("comments/{blogId}")]
        public IActionResult GetCommentsByBlogId(int blogId)
        {
            var comments = new List<object>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, blogId, userId, commentContent, createdAt, updatedAt FROM blogcomments WHERE blogId = @blogId", conn);
                cmd.Parameters.AddWithValue("@blogId", blogId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comments.Add(new
                        {
                            Id = reader.GetInt32("id"),
                            BlogId = reader.GetInt32("blogId"),
                            UserId = reader.GetInt32("userId"),
                            CommentContent = reader.GetString("commentContent"),
                            CreatedAt = reader.GetDateTime("createdAt"),
                            UpdatedAt = reader.GetDateTime("updatedAt")
                        });
                    }
                }
            }
            return Ok(comments);
        }

        [HttpGet("user")]
        public IActionResult GetBlogsByUserId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var blogs = new List<Dictionary<string, object>>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();

                // Lấy toàn bộ blog trước
                var cmd = new MySqlCommand("SELECT id, title, place, content, statusBlog, authorId, thumbnail, createdAt, updatedAt FROM blogs WHERE authorId = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", userId);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var blog = new Dictionary<string, object>
                        {
                            ["Id"] = reader.GetInt32("id"),
                            ["Title"] = reader.GetString("title"),
                            ["Place"] = reader.GetString("place"),
                            ["Content"] = reader.GetString("content"),
                            ["Status"] = reader.GetString("statusBlog"),
                            ["AuthorId"] = reader.GetInt32("authorId"),
                            ["Thumbnail"] = reader.GetInt32("thumbnail"),
                            ["CreatedAt"] = reader.GetDateTime("createdAt"),
                            ["UpdatedAt"] = reader.GetDateTime("updatedAt")
                        };
                        blogs.Add(blog);
                    }
                }

                // Sau khi đóng reader trước đó, giờ có thể mở truy vấn mới
                foreach (var blog in blogs)
                {
                    var blogId = (int)blog["Id"];
                    var mediaList = new List<object>();

                    using (var mediaCmd = new MySqlCommand("SELECT id, blogId, mediaUrl, mediaType, imageFor FROM blogmedia WHERE blogId = @blogId", conn))
                    {
                        mediaCmd.Parameters.AddWithValue("@blogId", blogId);
                        using (var mediaReader = mediaCmd.ExecuteReader())
                        {
                            while (mediaReader.Read())
                            {
                                mediaList.Add(new
                                {
                                    Id = mediaReader.GetInt32("id"),
                                    BlogId = mediaReader.GetInt32("blogId"),
                                    MediaUrl = mediaReader.GetString("mediaUrl"),
                                    MediaType = mediaReader.GetInt32("mediaType"),
                                    ImageFor = mediaReader.GetString("imageFor"),
                                });
                            }
                        }
                    }

                    blog["MediaList"] = mediaList;
                }
            }

            return Ok(blogs);
        }
        
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBlog(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM blogs WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);

                var result = cmd.ExecuteNonQuery();

                // Xóa tất cả các media liên quan đến blog
                var mediaCmd = new MySqlCommand("DELETE FROM blogmedia WHERE blogId = @id", conn);
                mediaCmd.Parameters.AddWithValue("@id", id);
                mediaCmd.ExecuteNonQuery();
                // Xóa tất cả các bình luận liên quan đến blog
                var commentCmd = new MySqlCommand("DELETE FROM blogcomments WHERE blogId = @id", conn);
                commentCmd.Parameters.AddWithValue("@id", id);
                commentCmd.ExecuteNonQuery();

                // xóa các file media trong thư mục uploads
                var mediaList = new List<string>();
                var getMediaCmd = new MySqlCommand("SELECT mediaUrl FROM blogmedia WHERE blogId = @id", conn);
                getMediaCmd.Parameters.AddWithValue("@id", id);
                using (var reader = getMediaCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var mediaUrl = reader.GetString("mediaUrl");
                        mediaList.Add(mediaUrl);
                    }
                }
                foreach (var mediaUrl in mediaList)
                {
                    var filePath = Path.Combine(_env.WebRootPath, mediaUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
                // Trả về kết quả
                if (result > 0)
                {
                    return Ok(new { message = "Blog deleted successfully." });
                }
                return NotFound(new { message = "Blog not found." });
            }
        }
    }

    public class CommentBlog
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int UserId { get; set; }
        public string CommentContent { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int Status { get; set; } = 0;
    }

    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public int Category { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Status { get; set; } = "draft";
        public int AuthorId { get; set; }
        public int? Thumbnail { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class UploadThumbnailRequest
    {
        public int BlogId { get; set; }
        public int Type { get; set; }
        public string ImageFor { get; set; } = string.Empty;
        public required IFormFile File { get; set; }
    }

    public class BlogMedia
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public int MediaType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ImageFor { get; set; } = string.Empty;
    }
}