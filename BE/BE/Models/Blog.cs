// file: Blog.cs
// Table: Blogs - database table
// CREATE TABLE IF NOT EXISTS `blogs` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `title` varchar(100) DEFAULT NULL,
//   `content` varchar(255) DEFAULT NULL,
//   `status` varchar(100) DEFAULT NULL,
//   `authorId` int DEFAULT NULL,
//   `thumbnail` int DEFAULT NULL,
//   `createdAt` timestamp NULL DEFAULT NULL,
//   `updatedAt` timestamp NULL DEFAULT NULL,
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

namespace BE.Models;
public class Blog
{
    
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Status { get; set; } = "draft";
    public int AuthorId { get; set; }
    public int Thumbnail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}