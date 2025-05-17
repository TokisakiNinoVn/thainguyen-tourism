// file: user/userController.cs

// table database:
// CREATE TABLE IF NOT EXISTS `users` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `email` char(255) DEFAULT NULL,
//   `password` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '0',
//   `role` varchar(50) DEFAULT NULL,
//   `photoURL` char(255) DEFAULT NULL,
//   `displayName` char(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT 'Người ẩn danh',
//   `createdAt` datetime DEFAULT NULL,
//   `isGoogleLogin` int DEFAULT NULL,
//   `tokenChat` int DEFAULT '10',
//   `isLink` int DEFAULT '0',
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;


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
using BCrypt.Net;

namespace Controllers.web.user
{
    [ApiController]
    [Route("api/web/user")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            Env.Load();
        }

        [HttpGet("get-infor")]
        public IActionResult GetUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var command = new MySqlCommand("SELECT email, photoURL, displayName, isGoogleLogin, tokenChat, isLink FROM users WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", userId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var user = new
                        {
                            Email = reader["email"],
                            DisplayName = reader["displayName"],
                            PhotoURL = reader["photoURL"],
                            IsGoogleLogin = reader["isGoogleLogin"],
                            TokenChat = reader["tokenChat"],
                            IsLink = reader["isLink"]

                        };
                        return Ok(user);
                    }
                }
            }
            return NotFound();
        }

        [HttpPost("update-infor")]
        public IActionResult UpdateUser([FromBody] UserWeb user)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var command = new MySqlCommand("UPDATE users SET displayName = @DisplayName, WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", userId);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@DisplayName", user.DisplayName);
                command.Parameters.AddWithValue("@PhotoURL", user.PhotoURL);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User updated successfully" });
                }
            }
            return NotFound();
        }

        // [HttpPost("link-account")] - Cái này là để liên kết tài khoản google với tài khoản của mình: là thay đổi mật khẩu
        [HttpPost("link-account")]
        public IActionResult LinkAccount([FromBody] UserWeb user)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            // Băm mật khẩu trước khi lưu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            // string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var command = new MySqlCommand("UPDATE users SET password = @Password, isLink = 1 WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", userId);
                command.Parameters.AddWithValue("@Password", hashedPassword);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User updated successfully" });
                }
            }

            return NotFound();
        }

        // [HttpPost("update-display-name")] - Cái này là để cập nhật tên hiển thị của tài khoản 
        [HttpPost("update-display-name")]
        public IActionResult UpdateDisplayName([FromBody] UserWeb user)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var command = new MySqlCommand("UPDATE users SET displayName = @DisplayName WHERE id = @id", conn);
                command.Parameters.AddWithValue("@id", userId);
                command.Parameters.AddWithValue("@DisplayName", user.DisplayName);

                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    return Ok(new { message = "User updated successfully" });
                }
            }
            return NotFound();
        }
    }
}


public class UserWeb
{
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string? PhotoURL { get; set; }
    public string? Role { get; set; } = "user";
    public DateTime CreatedAt { get; set; }
    public int IsGoogleLogin { get; set; }
    public int TokenChat { get; set; }
    public int IsLink { get; set; }
    public string Password { get; set; } = string.Empty;
}
