// Table: chatdata
// CREATE TABLE IF NOT EXISTS `chatdata` (
//   `id` int NOT NULL AUTO_INCREMENT,
//   `userId` int DEFAULT NULL,
//   `content` varchar(2000) NOT NULL,
//   `type` varchar(50) NOT NULL COMMENT 'request/response',
//   `responseChatId` int DEFAULT NULL,
//   `createdAt` timestamp NULL DEFAULT NULL,
//   PRIMARY KEY (`id`)
// ) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

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

namespace Controllers.web.chat
{
    [ApiController]
    [Route("api/web/chat")]
    public class ChatController : ControllerBase
    {
        static ChatController()
        {
            Env.Load();
        }

        private readonly ILogger<ChatController> _logger;
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiUrl = Env.GetString("API_URLCHATBOT");

        public ChatController(ILogger<ChatController> logger)
        {
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.Content))
                return BadRequest("Invalid message data.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not authenticated.");

            int requestId;

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();

                // Lưu tin nhắn request vào bảng chatdata với type = "request"
                var cmd = new MySqlCommand(
                    "INSERT INTO chatdata (userId, content, type, createdAt) VALUES (@userId, @content, 'request', @createdAt); SELECT LAST_INSERT_ID();",
                    conn
                );
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                cmd.Parameters.AddWithValue("@content", message.Content);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);

                requestId = Convert.ToInt32(cmd.ExecuteScalar());

                // Kiểm tra token chat của người dùng: nếu <= 0 thì chỉ phản hồi lỗi, KHÔNG trừ điểm và KHÔNG gọi API
                var checkTokenCmd = new MySqlCommand("SELECT tokenChat FROM users WHERE id = @userId", conn);
                checkTokenCmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                var tokenChat = (int)checkTokenCmd.ExecuteScalar();

                if (tokenChat <= 0)
                {
                    // Lưu tin nhắn vào bảng chatdata với type = "response" và responseChatId là id của request
                    var responseCmd = new MySqlCommand(
                        "INSERT INTO chatdata (userId, content, type, responseChatId, createdAt) VALUES (@userId, @content, 'response', @responseChatId, @createdAt)",
                        conn
                    );
                    responseCmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                    responseCmd.Parameters.AddWithValue("@content", "Bạn cần liên hệ admin nạp thêm token chat để sử dụng dịch vụ này.");
                    responseCmd.Parameters.AddWithValue("@responseChatId", requestId);
                    responseCmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                    responseCmd.ExecuteNonQuery();

                    return BadRequest(new
                    {
                        code = 400,
                        status = "error",
                        message = "Token để sử dụng chatbot của bạn đã hết! Bạn cần liên hệ admin nạp thêm token chat để sử dụng dịch vụ này.",
                        data = Array.Empty<object>()
                    });
                }

                // Trừ 1 điểm tokenChat
                var updateCmd = new MySqlCommand("UPDATE users SET tokenChat = tokenChat - 1 WHERE id = @userId", conn);
                updateCmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                updateCmd.ExecuteNonQuery();
            }

            // Gửi nội dung đến API
            string responseText = await GetChatResponse(message.Content);
            var responseJson = JsonDocument.Parse(responseText);
            if (responseJson.RootElement.TryGetProperty("choices", out var choices) && choices[0].TryGetProperty("text", out var text))
            {
                var errorText = "Xin lỗi, tôi không thể hiểu yêu cầu của bạn! Mô hình của tôi không đủ thông minh để xử lý yêu cầu này.";
                responseText = text.GetString() ?? errorText;
                if (string.IsNullOrEmpty(responseText) || responseText == errorText)
                {
                    responseText = errorText;
                }
            }
            else
            {
                responseText = "Xin lỗi, tôi không thể hiểu yêu cầu của bạn! Mô hình của tôi không đủ thông minh để xử lý yêu cầu này.";
            }

            // Lưu response vào bảng chatdata
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO chatdata (userId, content, type, responseChatId, createdAt) VALUES (@userId, @content, 'response', @responseChatId, @createdAt)",
                    conn
                );
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                cmd.Parameters.AddWithValue("@content", responseText);
                cmd.Parameters.AddWithValue("@responseChatId", requestId);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                cmd.ExecuteNonQuery();
            }

            return Ok(new
            {
                Request = message.Content,
                Response = responseText
            });
        }

        // Lấy tất cả nội dung cuộc chat của người dùng
        [HttpGet("history")]
        public IActionResult GetChatHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Bạn cần đăng nhập để sử dụng chức năng này.");

            var chatHistory = new List<object>();

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM chatdata WHERE userId = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chatHistory.Add(new
                        {
                            Id = reader["id"],
                            Content = reader["content"],
                            Type = reader["type"],
                            CreatedAt = reader["createdAt"]
                        });
                    }
                }

                // Nếu không có đoạn chat nào, thêm tin nhắn mặc định từ bot
                if (chatHistory.Count == 0)
                {
                    chatHistory.Add(new
                    {
                        Id = 0,
                        Content = "Hãy bắt đầu tin nhắn đầu tiên của bạn với chatbot!",
                        Type = "response",
                        userId = int.Parse(userId),
                        CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
                    });
                }
            }

            return Ok(chatHistory);
        }


        // Load token chat của người dùng
        [HttpGet("token")]
        public IActionResult GetTokenChat()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Bạn cần đăng nhập để sử dụng chức năng này.");

            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT tokenChat FROM users WHERE id = @userId", conn);
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                var tokenChat = (int)cmd.ExecuteScalar();

                return Ok(new
                {
                    code = 200,
                    status = "success",
                    message = "Lấy token chat thành công",
                    data = new { tokenChat }
                });
            }
        }

        // Lấy các câu hỏi chatbot mặc định
        [HttpGet("default-questions")]
        public IActionResult GetDefaultQuestions()
        {
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, question FROM chatdefault", conn);
                var defaultQuestions = new List<object>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        defaultQuestions.Add(new
                        {
                            Id = reader["id"],
                            Question = reader["question"],
                        });
                    }

                    return Ok(new
                    {
                        code = 200,
                        status = "success",
                        message = "Lấy câu hỏi mặc định thành công",
                        data = defaultQuestions
                    });
                }
            }
        }

        // trả lời các câu hỏi mặc định
        [HttpPost("default-questions/answer")]

        public IActionResult AnswerDefaultQuestion([FromBody] ChatMessage message)
        {
            if (message == null || string.IsNullOrEmpty(message.Content))
                return BadRequest("Invalid message data.");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not authenticated.");

            // Lưu tin nhắn request vào bảng chatdata với type = "request"
            int requestId;
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO chatdata (userId, content, type, createdAt) VALUES (@userId, @content, 'request', @createdAt); SELECT LAST_INSERT_ID();",
                    conn
                );
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                cmd.Parameters.AddWithValue("@content", message.Content);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);

                requestId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            // lấy câu trả lời từ bảng chatdefault
            string responseText;
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT answer FROM chatdefault WHERE question = @question", conn);
                cmd.Parameters.AddWithValue("@question", message.Content);
                var answer = cmd.ExecuteScalar();
                responseText = answer?.ToString() ?? "Xin lỗi, tôi không thể hiểu yêu cầu của bạn! Mô hình của tôi không đủ thông minh để xử lý yêu cầu này.";
            }

            // lưu thông tin vào bảng chatdata với type = "response"
            using (var conn = DbConfig.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO chatdata (userId, content, type, responseChatId, createdAt) VALUES (@userId, @content, 'response', @responseChatId, @createdAt)",
                    conn
                );
                cmd.Parameters.AddWithValue("@userId", int.Parse(userId));
                cmd.Parameters.AddWithValue("@content", responseText);
                cmd.Parameters.AddWithValue("@responseChatId", requestId);
                cmd.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);
                cmd.ExecuteNonQuery();
            }

            return Ok(new
            {
                Request = message.Content,
                Response = responseText
            });
        }

        private async Task<string> GetChatResponse(string prompt)
        {
            var payload = new
            {
                prompt = prompt,
                max_tokens = 150,
                temperature = 0.7,
                top_p = 1.0,
                frequency_penalty = 0.0,
                presence_penalty = 0.0,
                stop = new[] { "\n" }
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, jsonContent);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error while calling chat API: {Message}", httpEx.Message);
                return "Lỗi khi gọi API chatbot.";
            }
        }
    }

    public class ChatMessage
    {
        public required string Content { get; set; }
    }
}
