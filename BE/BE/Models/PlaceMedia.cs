// database
// CREATE TABLE IF NOT EXISTS `place_media` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `place_id` int DEFAULT NULL,
//   `media_url` varchar(255) DEFAULT NULL,
//   `media_type` int DEFAULT NULL,
//   `createdAt` datetime DEFAULT NULL,
//   `updatedAt` datetime DEFAULT NULL,
//   `image_for` varchar(255) DEFAULT NULL,
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


using System.Reflection.Metadata;

namespace BE.Models;

public class PlaceMedia
{
    public int Id { get; set; }
    public int PlaceId { get; set; }
    public string MediaUrl { get; set; } = string.Empty;
    public int MediaType { get; set; } // 1: image, 2: image360, 3: video, 4: audio
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public string ImageFor { get; set; } = string.Empty; // thumbnail, place media
}