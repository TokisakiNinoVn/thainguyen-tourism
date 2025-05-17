using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using dbConfig;
using System.Text.Json;

namespace Controllers.admin.dashboard
{
    [Route("api/admin/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }
        [HttpGet("visit-count")]
        public IActionResult GetVisitCount()
        {
            try
            {
                string filePath = "visit-counter.json";

                if (!System.IO.File.Exists(filePath))
                {
                    return Ok(new
                    {
                        code = 200,
                        status = "success",
                        message = "Thành công",
                        data = new List<object>()
                    });
                }

                var json = System.IO.File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(json))
                {
                    return Ok(new
                    {
                        code = 200,
                        status = "success",
                        message = "Thành công",
                        data = new List<object>()
                    });
                }

                Dictionary<string, int> counts;
                try
                {
                    counts = JsonSerializer.Deserialize<Dictionary<string, int>>(json) ?? new Dictionary<string, int>();
                }
                catch (JsonException)
                {
                    counts = new Dictionary<string, int>();
                }

                var now = DateTime.Now;

                var monthsToShow = Enumerable.Range(0, 5)
                    .Select(i => now.AddMonths(-i).ToString("MMyyyy"))
                    .ToList();

                var result = new List<object>();

                foreach (var month in monthsToShow)
                {
                    string key = $"count-{month}";
                    counts.TryGetValue(key, out int count);
                    result.Add(new { month, count });
                }

                result.Reverse();

                return Ok(new
                {
                    code = 200,
                    status = "success",
                    message = "Thành công",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy số người truy cập");
                return StatusCode(500, "Lỗi server");
            }
        }



        // [HttpGet("visit-count")]
        // public IActionResult GetVisitCount()
        // {
        //     try
        //     {
        //         int visitCount = 0;
        //         string filePath = "visit-counter.txt";

        //         if (System.IO.File.Exists(filePath))
        //         {
        //             string content = System.IO.File.ReadAllText(filePath);
        //             visitCount = int.Parse(content);
        //         }

        //         return Ok(new { visitCount });
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Lỗi khi lấy số người truy cập");
        //         return StatusCode(500, "Lỗi server");
        //     }
        // }

        [HttpGet("blog-count")]
        public IActionResult GetBlogCount()
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT COUNT(*) FROM blogs", connection);
                    int blogCount = Convert.ToInt32(command.ExecuteScalar());
                    return Ok(new { blogCount });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy số lượng bài viết");
                return StatusCode(500, "Lỗi server");
            }
        }

        [HttpGet("user-count")]
        public IActionResult GetUserCount()
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT COUNT(*) FROM users", connection);
                    int userCount = Convert.ToInt32(command.ExecuteScalar());
                    return Ok(new { userCount });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy số lượng người dùng");
                return StatusCode(500, "Lỗi server");
            }
        }

        [HttpGet("review-count")]
        public IActionResult GetCommentCount()
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand("SELECT COUNT(*) FROM reviews", connection);
                    int reviewCount = Convert.ToInt32(command.ExecuteScalar());
                    return Ok(new { reviewCount });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy số lượng reviews");
                return StatusCode(500, "Lỗi server");
            }
        }

        [HttpGet("favorite-places")]
        public IActionResult GetFavoritePlaces()
        {
            try
            {
                using (var connection = DbConfig.GetConnection())
                {
                    connection.Open();
                    var command = new MySqlCommand(
                        "SELECT places.name, COUNT(saveplace.id) AS count " +
                        "FROM saveplace " +
                        "JOIN places ON saveplace.placeId = places.id " +
                        "GROUP BY places.name " +
                        "ORDER BY count DESC", connection);

                    using (var reader = command.ExecuteReader())
                    {
                        var favoritePlaces = new List<object>();
                        while (reader.Read())
                        {
                            favoritePlaces.Add(new
                            {
                                name = reader["name"].ToString(),
                                count = Convert.ToInt32(reader["count"])
                            });
                        }
                        return Ok(favoritePlaces);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách địa điểm yêu thích");
                return StatusCode(500, "Lỗi server");
            }
        }
    }
}
