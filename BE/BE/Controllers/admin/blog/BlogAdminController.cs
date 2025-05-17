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
    [Route("api/admin/blog")]
    public class BlogAdminController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult GetListAllBlogs()
        {
            var blogs = new List<Dictionary<string, object>>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, title, place, content, authorId, thumbnail, createdAt, updatedAt, category, statusBlog FROM blogs", conn);
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
                            { "Category", reader.GetInt32("category") },
                            { "Status", reader.GetString("statusBlog") }
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

        // xóa bài viết và các media, comment liên quan
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBlog(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();

                // Lấy danh sách media liên quan đến blog
                var mediaList = new List<string>();
                var mediaCmd = new MySqlCommand("SELECT mediaUrl FROM blogmedia WHERE blogId = @blogId", conn);
                mediaCmd.Parameters.AddWithValue("@blogId", id);
                using (var mediaReader = mediaCmd.ExecuteReader())
                {
                    while (mediaReader.Read())
                    {
                        var mediaUrl = mediaReader.GetString("mediaUrl");
                        mediaList.Add(mediaUrl);
                    }
                }
                // Xóa các file media từ server
                foreach (var mediaUrl in mediaList)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", mediaUrl);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Xóa các media liên quan đến blog
                var deleteMediaCmd = new MySqlCommand("DELETE FROM blogmedia WHERE blogId = @blogId", conn);
                deleteMediaCmd.Parameters.AddWithValue("@blogId", id);
                deleteMediaCmd.ExecuteNonQuery();

                // Xóa bài viết
                var deleteBlogCmd = new MySqlCommand("DELETE FROM blogs WHERE id = @id", conn);
                deleteBlogCmd.Parameters.AddWithValue("@id", id);
                deleteBlogCmd.ExecuteNonQuery();
            }

            return Ok(new { message = "Xóa bài viết thành công." });
        }

        // Phê duyệt bài viết
        [HttpPut("approve/{id}")]
        public IActionResult ApproveBlog(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE blogs SET statusBlog = 'approved' WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return Ok(new { message = "Phê duyệt bài viết thành công." });
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
    }
}