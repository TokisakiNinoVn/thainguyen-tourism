// Database: Reviews
//  CREATE TABLE IF NOT EXISTS `reviews` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `placeId` int DEFAULT NULL,
//   `userId` int DEFAULT NULL,
//   `rating` float DEFAULT NULL,
//   `reviewDescription` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
//   `status` int DEFAULT NULL,
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
using System.Data;

namespace Controllers.admin.review
{
    [ApiController]
    [Route("api/admin/review")]
    public class ReviewAdminController : ControllerBase
    {
        static ReviewAdminController()
        {
            Env.Load();
        }

        private readonly ILogger<ReviewAdminController> _logger;


        public ReviewAdminController(ILogger<ReviewAdminController> logger)
        {
            _logger = logger;
        }


        [HttpGet("all")]
        public async Task<IActionResult> GetAllReviews()
        {
            try
            {
                var reviews = new List<Review>();
                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    var command = new MySqlCommand("SELECT * FROM reviews", connection);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var review = new Review
                            {
                                Id = reader.GetInt32("id"),
                                PlaceId = reader.GetInt32("placeId"),
                                UserId = reader.GetInt32("userId"),
                                Rating = reader.GetFloat("rating"),
                                ReviewDescription = reader.GetString("reviewDescription"),
                                Status = reader.GetInt32("status"),
                                CreatedAt = reader.GetDateTime("createdAt"),
                                UpdatedAt = reader.GetDateTime("updatedAt")
                            };
                            reviews.Add(review);
                        }
                    }
                }
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving reviews");
                return StatusCode(500, "Internal server error");
            }
        }

        // Duyệt đánh giá
        [HttpPost("approve/{id}")]
        public async Task<IActionResult> ApproveReview(int id)
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    var command = new MySqlCommand("UPDATE reviews SET status = 1 WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
                return Ok(new { message = "Review approved successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving review");
                return StatusCode(500, "Internal server error");
            }
        }
        // Xóa đánh giá
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    var command = new MySqlCommand("DELETE FROM reviews WHERE id = @id", connection);
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
                return Ok(new { message = "Review deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting review");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    public class Review
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int UserId { get; set; }
        public float Rating { get; set; }
        public string? ReviewDescription { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}