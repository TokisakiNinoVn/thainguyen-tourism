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

namespace Controllers.web.review
{
    [ApiController]
    [Route("api/web/review")]
    public class ReviewController : ControllerBase
    {
        static ReviewController()
        {
            Env.Load();
        }

        private readonly ILogger<ReviewController> _logger;


        public ReviewController(ILogger<ReviewController> logger)
        {
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> cteateReview([FromBody] Review review)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not authenticated.");
                }

                review.UserId = int.Parse(userId);
                review.CreatedAt = DateTime.UtcNow;
                review.UpdatedAt = DateTime.UtcNow;

                using (var connection =  DbConfig.GetConnection())
                {
                    await connection.OpenAsync();
                    var query = "INSERT INTO reviews (placeId, userId, rating, reviewDescription, createdAt, updatedAt) VALUES (@placeId, @userId, @rating, @reviewDescription, @createdAt, @updatedAt)";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@placeId", review.PlaceId);
                        command.Parameters.AddWithValue("@userId", review.UserId);
                        command.Parameters.AddWithValue("@rating", review.Rating);
                        command.Parameters.AddWithValue("@reviewDescription", review.ReviewDescription);
                        command.Parameters.AddWithValue("@createdAt", review.CreatedAt);
                        command.Parameters.AddWithValue("@updatedAt", review.UpdatedAt);

                        await command.ExecuteNonQueryAsync();
                    }
                }

                return Ok(new { message = "Review created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating review");
                return StatusCode(500, "Internal server error");
            }
        }
        
    }

    public class Review
    {
        public int Id { get; set; }
        public int PlaceId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public required string ReviewDescription { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}