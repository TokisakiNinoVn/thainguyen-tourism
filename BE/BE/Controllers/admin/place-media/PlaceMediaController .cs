
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
            Console.WriteLine("UploadSingleRequest: " + request.ToString());
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
                UpdatedAt = DateTime.UtcNow,
                ImageFor = request.ImageFor // thumbnail, place media
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

        // GET: api/PlaceMedia/all/:id - get all media for a place
        [HttpGet("all/{id}")]
        public async Task<IActionResult> GetAll(int id) // hoặc [FromRoute] int id
        {
            var media = await _context.PlaceMedia
                .Where(m => m.PlaceId == id && m.ImageFor == "place-media")
                .ToListAsync();

            if (media == null || media.Count == 0)
                return NotFound("No media found for this place");

            return Ok(media);
        }


        // DELETE: api/PlaceMedia/delete/{id} - delete a media by id
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var media = await _context.PlaceMedia.FindAsync(id);
            if (media == null)
                return NotFound("Media not found");

            // Delete the file from the server
            var filePath = Path.Combine(_env.WebRootPath, media.MediaUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.PlaceMedia.Remove(media);
            await _context.SaveChangesAsync();

            return Ok("Media deleted successfully");
        }
    }

}
