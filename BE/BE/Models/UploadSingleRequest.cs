namespace BE.Models;

public class UploadSingleRequest
{
    public int PlaceId { get; set; }
    public int Type { get; set; }
    public string ImageFor { get; set; } = string.Empty; // thumbnail, place media
    public required IFormFile File { get; set; }
}
