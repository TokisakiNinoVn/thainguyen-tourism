// Model/User.cs
// Table: Users - database table
// CREATE TABLE IF NOT EXISTS `users` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `email` char(255) DEFAULT NULL,
//   `password` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '0',
//   `role` varchar(50) DEFAULT NULL,
//   `photoURL` char(255) DEFAULT NULL,
//   `displayName` char(255) DEFAULT NULL,
//   `createdAt` timestamp NULL DEFAULT NULL,
//   `isGoogleLogin` int DEFAULT NULL,
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
namespace BE.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    // public required string? Password { get; set; }
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "user";
    public string? PhotoURL { get; set; }
    public string? DisplayName { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public int IsGoogleLogin { get; set; } = 0;
}
