// Table database: Blog 
// CREATE TABLE IF NOT EXISTS `blogs` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `title` varchar(100) DEFAULT NULL,
//   `content` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
//   `status` varchar(100) DEFAULT NULL,
//   `authorId` int DEFAULT NULL,
//   `thumbnail` int DEFAULT NULL,
//   `createdAt` timestamp NULL DEFAULT NULL,
//   `updatedAt` timestamp NULL DEFAULT NULL,
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using dbConfig; // nơi bạn đặt DbConfig

[ApiController]
[Route("api/web/blog")]
public class BlogControllerWeb : ControllerBase
{
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
            var cmd = new MySqlCommand("SELECT * FROM blogs WHERE id = @id", conn);
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

    // Lấy các review của blog theo id
    [HttpGet("reviews/{id}")]
    public IActionResult GetReviewsByBlogId(int id)
    {
        var reviews = new List<object>();

        using (var conn = DbConfig.GetConnection())
        {
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM reviews WHERE blogId = @blogId", conn);
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
}
