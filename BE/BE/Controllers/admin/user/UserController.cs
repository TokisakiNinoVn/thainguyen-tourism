using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.Security.Claims;
using dbConfig;
using System.Data;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/admin/user")]
    public class UserController : ControllerBase
    {
        // Lấy danh sách tất cả người dùng
        [HttpGet("list")]
        public IActionResult GetListAllUser()
        {
            var users = new List<Dictionary<string, object>>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM users WHERE role = 'user'", conn);

                Console.WriteLine("Query executed successfully.");
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var user = new Dictionary<string, object>
                        {
                            { "id", reader["id"] },
                            { "status", reader["status"] },
                            { "display_name", reader["displayName"] },
                            { "email", reader["email"] },
                            { "password", reader["password"] },
                            // { "phone", reader["phone"] },
                            { "created_at", reader["createdAt"] },
                            // { "updated_at", reader["updated_at"] }
                        };
                        users.Add(user);
                    }
                }
            }

            return Ok(users);
        }

        // Khóa tài khoản người dùng set status = 0
        [HttpPut("lock/{id}")]
        public IActionResult LockUser(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE users SET status = 0 WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return Ok(new { message = "User locked successfully" });
        }

        // Mở khóa tài khoản người dùng set status = 1
        [HttpPut("unlock/{id}")]
        public IActionResult UnlockUser(int id)
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE users SET status = 1 WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return Ok(new { message = "User unlocked successfully" });
        }


        public class UserAdmin
        {
            public int Id { get; set; }
            public int Status { get; set; }
            public string DisplayName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            // public string Phone { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
        }
    }
}