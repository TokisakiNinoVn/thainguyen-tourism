// table: reviews
//  CREATE TABLE IF NOT EXISTS `reviews` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `placeId` int DEFAULT NULL,
//   `userId` int DEFAULT NULL,
//   `rating` int DEFAULT NULL,
//   `reviewDescription` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
//   `createdAt` timestamp NULL DEFAULT NULL,
//   `updatedAt` timestamp NULL DEFAULT NULL,
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

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
    [Route("api/web/place")]
    [ApiController]
    public class PlaceWebController : ControllerBase
    {
        private readonly ILogger<PlaceWebController> _logger;
        private readonly string _connectionString;

        public PlaceWebController(ILogger<PlaceWebController> logger)
        {
            _logger = logger;
            Env.Load();
            _connectionString = Env.GetString("DB_CONNECTION_STRING");
        }

        // [HttpGet("review/{id}")]
        [HttpGet("review/{placeId}")]
        public async Task<IActionResult> GetReviews(int placeId)
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();

                    var reviews = new List<object>();

                    // Lấy tất cả đánh giá cho địa điểm theo placeId
                    using (var command = new MySqlCommand(@"
                        SELECT r.reviewDescription, r.rating, r.createdAt, u.displayName 
                        FROM reviews r
                        LEFT JOIN users u ON r.userId = u.id
                        WHERE r.placeId = @placeId and r.status = '1'
                        ORDER BY r.createdAt DESC", connection))
                    {
                        command.Parameters.AddWithValue("@placeId", placeId);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var review = new
                                {
                                    Content = reader["reviewDescription"],
                                    Rating = reader["rating"],
                                    CreatedAt = reader["createdAt"],
                                    DisplayName = reader["displayName"]
                                };

                                reviews.Add(review);
                            }
                        }
                    }

                    return Ok(reviews);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching reviews for placeId {PlaceId}", placeId);
                return StatusCode(500, new { Message = "Internal server error" });
            }
        }

        // // Lưu địa điểm/blog với Id của place và id của user
        // [HttpPost("save")]
        // public async Task<IActionResult> SavePlace([FromBody] object place)
        // {
        //     var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     if (userId == null)
        //     {
        //         return Unauthorized();
        //     }

        //     try
        //     {
        //         using (var connection = DbConfig.GetConnection())
        //         {
        //             await connection.OpenAsync();

        //             // Lưu địa điểm vào cơ sở dữ liệu
        //             using (var command = new MySqlCommand("INSERT INTO likesplace (placeId, userId) VALUES (@placeId, @userId)", connection))
        //             {
        //                 command.Parameters.AddWithValue("@placeId", place);
        //                 command.Parameters.AddWithValue("@userId", userId);

        //                 await command.ExecuteNonQueryAsync();
        //             }

        //             return Ok(new { Message = "Place saved successfully" });
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error saving place");
        //         return StatusCode(500, new { Message = "Internal server error" });
        //     }
        // }

        
    }
}