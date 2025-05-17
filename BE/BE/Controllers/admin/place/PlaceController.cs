namespace BE.Controllers.admin.Place;

using BE;
using BE.Models;
// using BE.Models.PlaceMedia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using dbConfig;
using DotNetEnv;

[ApiController]
// [Authorize(Roles = "admin")]
[Route("api/admin/place")]

public class PlaceController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public PlaceController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // Lấy danh sách địa điểm
    [HttpGet("list")]
    public IActionResult GetAllPlaces()
    {
        var places = _context.Places.ToList();

        var placeDtos = places.Select(place =>
        {
            string? imageUrl = null;

            if (place.Thumbnail.HasValue)
            {
                var media = _context.PlaceMedia
                    .FirstOrDefault(pm => pm.Id == place.Thumbnail.Value);

                if (media != null)
                {
                    imageUrl = media.MediaUrl;
                }
            }

            var reviews = _context.Reviews
                .Where(r => r.PlaceId == place.Id && r.Status == 1)
                .ToList();

            double averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            int totalReviews = reviews.Count;

            return new
            {
                place.Id,
                place.Name,
                place.Description,
                place.Latitude,
                place.Longitude,
                place.Thumbnail,
                place.CreatedAt,
                place.Province,
                ImageUrl = imageUrl,
                AverageRating = averageRating,
                TotalReviews = totalReviews
            };
        }).ToList();


        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Lấy danh sách địa điểm thành công",
            data = placeDtos
        });
    }


    [HttpGet("list/basic")]
    public IActionResult GetListBasicPlaces()
    {
        var places = _context.Places.ToList();
        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Lấy danh sách địa điểm thành công",
            data = new
            {
                places = places.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Latitude,
                    p.Longitude,
                    p.Province,
                    p.Thumbnail
                }).ToList()
            }
        });
    }

    [HttpGet("{id}")]
    public IActionResult GetPlaceById(int id)
    {
        var place = _context.Places.FirstOrDefault(p => p.Id == id);
        if (place == null)
        {
            return NotFound(new
            {
                code = 404,
                status = "error",
                message = "Địa điểm không tồn tại",
                data = (object?)null
            });
        }

        // Lấy thông tin ảnh qua bảng PlaceMedia nếu giá trị Thumbnail khác null
        string? imageUrl = null;
        if (place.Thumbnail.HasValue)
        {
            var placeImage = _context.PlaceMedia
                .FirstOrDefault(pi => pi.PlaceId == place.Id && pi.MediaType == 1);
            if (placeImage != null)
            {
                imageUrl = placeImage.MediaUrl;
            }
        }

        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Lấy địa điểm thành công",
            data = new
            {
                place.Id,
                place.Name,
                place.Description,
                place.Latitude,
                place.Longitude,
                place.Thumbnail,
                place.Province,
                ImageUrl = imageUrl // Trả về URL ảnh trong đối tượng ẩn danh
            }
        });
    }

    [HttpPost("create")]
    public IActionResult CreatePlace([FromBody] Place place)
    {
        if (place == null || string.IsNullOrEmpty(place.Name))
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Tên địa điểm không được để trống",
                data = (object?)null
            });
        }

        _context.Places.Add(place);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetPlaceById), new { id = place.Id }, new
        {
            code = 201,
            status = "success",
            message = "Tạo địa điểm thành công",
            data = place
        });
    }

    [HttpPut("update/{id}")]
    public IActionResult UpdatePlace(int id, [FromBody] Place place)
    {
        if (place == null || string.IsNullOrEmpty(place.Name))
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Tên địa điểm không được để trống",
                data = (object?)null
            });
        }

        var existingPlace = _context.Places.FirstOrDefault(p => p.Id == id);
        if (existingPlace == null)
        {
            return NotFound(new
            {
                code = 404,
                status = "error",
                message = "Địa điểm không tồn tại",
                data = (object?)null
            });
        }

        existingPlace.Name = place.Name;
        existingPlace.Description = place.Description;
        existingPlace.Latitude = place.Latitude;
        existingPlace.Longitude = place.Longitude;
        // existingPlace.Thumbnail = place.Thumbnail;
        existingPlace.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();

        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Cập nhật địa điểm thành công",
            data = existingPlace
        });
    }
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePlace(int id)
    {
        var place = await _context.Places.FirstOrDefaultAsync(p => p.Id == id);
        if (place == null)
        {
            return NotFound(new
            {
                code = 404,
                status = "error",
                message = "Địa điểm không tồn tại",
                data = (object?)null
            });
        }

        // Xóa ảnh thumbnail nếu có
        if (place.Thumbnail.HasValue)
        {
            var thumbnail = await _context.PlaceMedia.FirstOrDefaultAsync(pm => pm.Id == place.Thumbnail.Value);
            if (thumbnail != null)
            {
                // Xóa file thumbnail khỏi server
                var filePath = Path.Combine(_env.WebRootPath, thumbnail.MediaUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.PlaceMedia.Remove(thumbnail);
            }
        }

        // Xóa ảnh/video/audio/... khác nếu có
        var mediaList = await _context.PlaceMedia.Where(pm => pm.PlaceId == id).ToListAsync();
        foreach (var media in mediaList)
        {
            // Xóa file khỏi server
            var filePath = Path.Combine(_env.WebRootPath, media.MediaUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.PlaceMedia.Remove(media);
        }

        // Xóa địa điểm
        _context.Places.Remove(place);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Xóa địa điểm thành công",
            data = (object?)null
        });
    }

    // Cập nhật thumbnail dựa vào id với id của ảnh thumbnail được truyền theo req body Thumbnail = 1
    // [HttpPost("update/thumbnail/{id}")]
    // public IActionResult UpdateThumbnail(int id, [FromBody] int thumbnailId)
    // {
    //     Console.WriteLine(thumbnailId);
    //     Console.WriteLine(id);

    //     var place = _context.Places.FirstOrDefault(p => p.Id == id);
    //     if (place == null)
    //     {
    //         return NotFound(new
    //         {
    //             code = 404,
    //             status = "error",
    //             message = "Địa điểm không tồn tại",
    //             data = (object?)null
    //         });
    //     }

    //     place.Thumbnail = thumbnailId;
    //     _context.SaveChanges();

    //     // Xóa ảnh cũ nếu có
    //     var oldThumbnail = _context.PlaceMedia.FirstOrDefault(pm => pm.PlaceId == id && pm.MediaType == 1);
    //     if (oldThumbnail != null)
    //     {
    //         _context.PlaceMedia.Remove(oldThumbnail);
    //         _context.SaveChanges();
    //     }

    //     return Ok(new
    //     {
    //         code = 200,
    //         status = "success",
    //         message = "Cập nhật thumbnail thành công",
    //         data = place
    //     });   
    // }

    [HttpPost("update/thumbnail/{id}")]
    public IActionResult UpdateThumbnail(int id, [FromBody] int thumbnailId)
    {
        var place = _context.Places.FirstOrDefault(p => p.Id == id);
        if (place == null)
        {
            return NotFound(new
            {
                code = 404,
                status = "error",
                message = "Địa điểm không tồn tại",
                data = (object?)null
            });
        }

        var newThumbnail = _context.PlaceMedia.FirstOrDefault(pm =>
            pm.Id == thumbnailId &&
            pm.PlaceId == id &&
            pm.MediaType == 1 &&
            pm.ImageFor == "thumbnail");

        if (newThumbnail == null)
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Thumbnail mới không hợp lệ",
                data = (object?)null
            });
        }

        // Xóa ảnh thumbnail cũ nếu khác với ảnh mới
        if (place.Thumbnail != null && place.Thumbnail != thumbnailId)
        {
            var oldThumbnail = _context.PlaceMedia.FirstOrDefault(pm =>
                pm.Id == place.Thumbnail &&
                pm.PlaceId == id &&
                pm.MediaType == 1 &&
                pm.ImageFor == "thumbnail");

            if (oldThumbnail != null)
            {
                _context.PlaceMedia.Remove(oldThumbnail);
            }
        }

        // Gán thumbnail mới
        place.Thumbnail = thumbnailId;
        place.UpdatedAt = DateTime.UtcNow;

        _context.SaveChanges();

        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Cập nhật thumbnail thành công",
            data = place
        });
    }

    // lấy danh danh sách địa trường Belong
    // [HttpGet("province")]
    // public IActionResult GetPlacesByBelong([FromQuery] string province)
    // {
    //     var places = _context.Places.Where(p => p.Province == province).ToList();
    //     if (places == null || places.Count == 0)
    //     {
    //         return NotFound(new
    //         {
    //             code = 200,
    //             status = "success",
    //             message = "Không tìm thấy địa điểm nào",
    //             data = new object[0]
    //         });
    //     }

    //     return Ok(new
    //     {
    //         code = 200,
    //         status = "success",
    //         message = "Lấy danh sách địa điểm thành công",
    //         data = places
    //     });
    // }
    // Lấy danh sách địa điểm theo tỉnh
    [HttpGet("province")]
    public IActionResult GetPlacesByProvince([FromQuery] string province)
    {
        var places = _context.Places.Where(p => p.Province == province).ToList();

        if (places == null || places.Count == 0)
        {
            return NotFound(new
            {
                code = 404, // Changed to 404 for NotFound
                status = "error", // Changed to "error" for better semantics
                message = "Không tìm thấy địa điểm nào cho tỉnh này.",
                data = new List<object>() // Use List<object> for consistency
            });
        }

        var placeDtos = places.Select(place =>
        {
            string? imageUrl = null;

            if (place.Thumbnail.HasValue)
            {
                var media = _context.PlaceMedia
                    .FirstOrDefault(pm => pm.Id == place.Thumbnail.Value);

                if (media != null)
                {
                    imageUrl = media.MediaUrl;
                }
            }

            return new
            {
                place.Id,
                place.Name,
                place.Description,
                place.Latitude,
                place.Longitude,
                place.Thumbnail,
                place.CreatedAt,
                place.Province,
                ImageUrl = imageUrl
            };
        }).ToList();

        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Lấy danh sách địa điểm thành công",
            data = placeDtos
        });
    }

    // [HttpGet("search")]
    // public IActionResult SearchPlaces([FromQuery] string query)
    // {
    //     var places = _context.Places
    //         .Where(p => !string.IsNullOrEmpty(query) && (p.Name.Contains(query) || p.Description.Contains(query)))
    //         .ToList();

    //     return Ok(new
    //     {
    //         code = 200,
    //         status = "success",
    //         message = "Tìm kiếm địa điểm thành công",
    //         data = places
    //     });
    // }
    // [HttpGet("filter")]
    // public IActionResult FilterPlaces([FromQuery] string? name, [FromQuery] string? description, [FromQuery] string? latitude, [FromQuery] string? longitude)
    // {
    //     var places = _context.Places.AsQueryable();

    //     if (!string.IsNullOrEmpty(name))
    //     {
    //         places = places.Where(p => p.Name.Contains(name));
    //     }

    //     if (!string.IsNullOrEmpty(description))
    //     {
    //         places = places.Where(p => p.Description.Contains(description));
    //     }

    //     if (double.TryParse(latitude, out double lat))
    //     {
    //         places = places.Where(p => p.Latitude == lat);
    //     }

    //     if (double.TryParse(longitude, out double lon))
    //     {
    //         places = places.Where(p => p.Longitude == lon);
    //     }

    //     return Ok(new
    //     {
    //         code = 200,
    //         status = "success",
    //         message = "Lọc địa điểm thành công",
    //         data = places.ToList()
    //     });
    // }
}