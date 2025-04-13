namespace BE.Models;

public class UploadSingleRequest
{
    public int PlaceId { get; set; }
    public int Type { get; set; }
    public required IFormFile File { get; set; }
}
