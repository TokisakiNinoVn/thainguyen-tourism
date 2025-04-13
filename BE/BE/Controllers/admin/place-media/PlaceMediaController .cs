
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using BE.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/admin/place-media")]
    public class PlaceMediaController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public PlaceMediaController(IWebHostEnvironment env, AppDbContext context)
        {
            _env = env;
            _context = context;
        }

        // POST: api/PlaceMedia/upload-single
        [HttpPost("upload-single")]
        public async Task<IActionResult> UploadSingle([FromForm] UploadSingleRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("File is empty");

            if (request.Type < 1 || request.Type > 4)
                return BadRequest("Invalid media type");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(request.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.File.CopyToAsync(stream);
            }

            var media = new PlaceMedia
            {
                PlaceId = request.PlaceId,
                MediaUrl = $"/uploads/{fileName}",
                MediaType = request.Type,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PlaceMedia.Add(media);
            await _context.SaveChangesAsync();

            return Ok(media);
        }


        // POST: api/PlaceMedia/upload-multiple
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple([FromForm] int placeId, [FromForm] int type, [FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded");

            if (type < 1 || type > 4)
                return BadRequest("Invalid media type (1: image, 2: image360, 3: video, 4: audio)");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var results = new List<PlaceMedia>();

            foreach (var file in files)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var media = new PlaceMedia
                {
                    PlaceId = placeId,
                    MediaUrl = $"/uploads/{fileName}",
                    MediaType = type,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.PlaceMedia.Add(media);
                results.Add(media);
            }

            await _context.SaveChangesAsync();
            return Ok(results);
        }
    }
}
