// table: 

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using dbConfig;
using DotNetEnv;

namespace Controllers.web.place
{
    [Route("api/web/save")]
    [ApiController]
    public class SaveController : ControllerBase
    {
        private readonly ILogger<SaveController> _logger;
        private readonly string _connectionString;

        public SaveController(ILogger<SaveController> logger)
        {
            _logger = logger;
            Env.Load();
            _connectionString = Env.GetString("DB_CONNECTION_STRING");
        }


        [HttpPost("save")]
        public async Task<IActionResult> SavePlace([FromBody] JsonElement placeJson)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                int placeId = placeJson.GetProperty("placeId").GetInt32();

                if (placeId == 0)
                {
                    return BadRequest(new { Message = "placeId is required" });
                }

                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();

                    // Kiểm tra xem đã lưu trước đó chưa
                    using (var checkCommand = new MySqlCommand("SELECT COUNT(*) FROM saveplace WHERE placeId = @placeId AND userId = @userId", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@placeId", placeId);
                        checkCommand.Parameters.AddWithValue("@userId", userId);

                        var count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());
                        if (count > 0)
                        {
                            return Ok(new { Message = "Place already saved" });
                        }
                    }

                    // Nếu chưa lưu thì thực hiện lưu
                    using (var command = new MySqlCommand("INSERT INTO saveplace (placeId, userId) VALUES (@placeId, @userId)", connection))
                    {
                        command.Parameters.AddWithValue("@placeId", placeId);
                        command.Parameters.AddWithValue("@userId", userId);

                        await command.ExecuteNonQueryAsync();
                    }

                    return Ok(new { Message = "Place saved successfully" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving place");
                return StatusCode(500, new { Message = "Internal server error " });
            }
        }


        [HttpGet("saved-places")]
        public async Task<IActionResult> GetSavedPlaces()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();

                    // Lấy danh sách địa điểm đã lưu, cùng thông tin từ bảng places và ảnh thumbnail
                    string query = @"
                        SELECT 
                            p.id AS placeId,
                            p.name AS PlaceName,
                            p.description AS Description,
                            p.latitude AS Latitude,
                            p.longitude AS Longitude,
                            p.province AS Province,
                            p.thumbnail AS Thumbnail,
                            pm.mediaUrl AS ThumbnailUrl
                        FROM saveplace sp
                        JOIN places p ON sp.placeId = p.id
                        LEFT JOIN placeMedia pm ON p.thumbnail = pm.id
                        WHERE sp.userId = @userId";

                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var savedPlaces = new List<object>();

                            while (await reader.ReadAsync())
                            {
                                savedPlaces.Add(new
                                {
                                    placeId = reader["PlaceId"],
                                    placeName = reader["PlaceName"],
                                    description = reader["Description"],
                                    thumbnailUrl = reader["ThumbnailUrl"]
                                });
                            }

                            return Ok(savedPlaces);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching saved places");
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        [HttpDelete("unsave/{placeId}")]
        public async Task<IActionResult> UnsavePlace(int placeId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            if (placeId == 0)
            {
                return BadRequest(new { Message = "placeId is required" });
            }

            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();

                    using (var command = new MySqlCommand("DELETE FROM saveplace WHERE placeId = @placeId AND userId = @userId", connection))
                    {
                        command.Parameters.AddWithValue("@placeId", placeId);
                        command.Parameters.AddWithValue("@userId", userId);

                        await command.ExecuteNonQueryAsync();
                    }

                    return Ok(new { Message = "Place unsaved successfully" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unsaving place");
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

    }
}