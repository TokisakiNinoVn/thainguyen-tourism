// database
// CREATE TABLE Places (
//     Id INT AUTO_INCREMENT PRIMARY KEY,
//     Name VARCHAR(255) NOT NULL,
//     Description TEXT,
//     Latitude DOUBLE,
//     Longitude DOUBLE,
//     Thumbnail VARCHAR(255), //     -- Thumbnail is an ID of the image in the table PlaceMedia
//     CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
//     UpdatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
// );

namespace BE.Models;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double? Latitude { get; set; } // demo: 21.814645988656277
    public double? Longitude { get; set; } // demo: 105.99999999999999
    public int? Thumbnail { get; set; } // demo: 1 (id of the image in the database)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
}