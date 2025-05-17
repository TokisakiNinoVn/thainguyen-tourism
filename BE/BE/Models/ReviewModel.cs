// // file: ReviewModel.cs

namespace BE.Models;
public class Review
{
    
    public int Id { get; set; }
    public int PlaceId { get; set; }
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string? ReviewDescription { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}