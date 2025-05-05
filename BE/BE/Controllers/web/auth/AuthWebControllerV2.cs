using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using dbConfig;
using DotNetEnv;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;

namespace Controllers.web.authv2
{
    [ApiController]
    [Route("api/web/auth")]
    public class AuthWebControllerV2 : ControllerBase
    {
        static AuthWebControllerV2()
        {
            Env.Load();
        }

        private readonly ILogger<AuthWebControllerV2> _logger;

        public AuthWebControllerV2(ILogger<AuthWebControllerV2> logger)
        {
            _logger = logger;
        }

        // POST: api/web/auth/google-login
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return BadRequest(new { message = "Email không được để trống" });

            using (var conn = DbConfig.GetConnection())
            {
                await conn.OpenAsync();
                var cmd = new MySqlCommand("SELECT id, role FROM users WHERE email = @Email AND isGoogleLogin = 1", conn);
                cmd.Parameters.AddWithValue("@Email", model.Email);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (!reader.Read())
                    {
                        return BadRequest(new { message = "Email chưa được đăng ký hoặc không đăng nhập bằng Google" });
                    }

                    var userId = reader.GetInt32("id");
                    var role = reader.IsDBNull(reader.GetOrdinal("role")) ? "user" : reader.GetString("role");

                    // JWT config
                    var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY is not configured.");
                    var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
                    var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

                    var key = Encoding.UTF8.GetBytes(jwtKey);
                    var expiration = TimeSpan.FromHours(3);

                    // Tạo token JWT
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                            new Claim(ClaimTypes.Email, model.Email),
                            new Claim(ClaimTypes.Role, role)
                        }),
                        Expires = DateTime.UtcNow.Add(expiration),
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256
                        )
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new
                    {
                        code = 200,
                        status = "success",
                        message = "Đăng nhập thành công",
                        data = new
                        {
                            token = tokenString,
                            // role = role
                        }
                    });
                }
            }
        }

        public class GoogleLoginRequest
        {
            public required string Email { get; set; }
        }
    }
}
