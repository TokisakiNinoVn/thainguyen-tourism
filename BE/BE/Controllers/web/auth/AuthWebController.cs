namespace BE.Controllers.Web.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BE.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

[Route("api/web/auth")]
[ApiController]
public class AuthWebController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthWebController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Email không tồn tại"
            });
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Tài khoản chưa có mật khẩu"
            });
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

        if (!isPasswordValid)
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Mật khẩu không đúng"
            });
        }

        // ===== Lấy JWT config =====
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY")
                    ?? throw new InvalidOperationException("JWT_KEY is not configured.");
        var key = Encoding.UTF8.GetBytes(jwtKey);
        var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var expiration = TimeSpan.FromHours(3);

        if (_config["JWT_EXPIRATION"]?.EndsWith("h") == true)
        {
            if (int.TryParse(_config["JWT_EXPIRATION"]![..^1], out var hours))
                expiration = TimeSpan.FromHours(hours);
        }

        // ===== Tạo JWT token =====
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role ?? "user")
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
                role = user.Role
            }
        });
    }


    // POST: api/AuthWeb/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        if (await _db.Users.AnyAsync(u => u.Email == model.Email))
            return BadRequest(new { message = "Email đã tồn tại" });

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var user = new User
        {
            Email = model.Email,
            Password = hashedPassword,
            DisplayName = model.DisplayName,
            CreatedAt = DateTime.UtcNow,
            IsGoogleLogin = 0,
            Role = "user",
            PhotoURL = "https://i.pinimg.com/736x/bd/61/ab/bd61ab78ce895f9e57744fe19040253c.jpg"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Đăng ký thành công" });
    }

    // POST: api/AuthWeb/forgot-password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null)
            return BadRequest(new { message = "Email không tồn tại" });

        return Ok(new { message = "Email khôi phục đã được gửi (giả lập)" });
    }

    // POST: api/AuthWeb/google-login
    [HttpPost("first-google-login")]
    public async Task<IActionResult> FirstGoogleLogin([FromBody] GoogleUserRequest model)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

        if (user == null)
        {
            user = new User
            {
                Email = model.Email,
                DisplayName = model.Name,
                PhotoURL = model.Avatar,
                IsGoogleLogin = 1,
                CreatedAt = DateTime.UtcNow,
                Password = "__google_login__" // hoặc null nếu DB cho phép
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Người dùng mới đã được tạo", data = user });
        }

        return Ok(new { message = "Đăng nhập bằng Google thành công", data = user });
    }

    // // POST: api/AuthWeb/google-login
    // [HttpPost("google-login")]
    // public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest model)
    // {
    //     var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

    //     if (user == null)
    //         return BadRequest(new { message = "Người dùng không tồn tại" });

    //     if (user.IsGoogleLogin != 1)
    //         return BadRequest(new { message = "Người dùng không đăng nhập bằng Google" });
    //     // Tạo JWT token cho người dùng đã đăng nhập bằng Google
    //     var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY") ?? throw new InvalidOperationException("JWT_KEY is not configured.");
    //     var key = Encoding.UTF8.GetBytes(jwtKey);
    //     var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
    //     var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
    //     var expiration = TimeSpan.FromHours(3);

    //     if (_config["JWT_EXPIRATION"]?.EndsWith("h") == true)
    //     {
    //         if (int.TryParse(_config["JWT_EXPIRATION"]![..^1], out var hours))
    //             expiration = TimeSpan.FromHours(hours);
    //     }

    //     // ===== Tạo JWT token =====
    //     var tokenHandler = new JwtSecurityTokenHandler();
    //     var tokenDescriptor = new SecurityTokenDescriptor
    //     {
    //         Subject = new ClaimsIdentity(new[]
    //         {
    //             new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
    //             new Claim(ClaimTypes.Email, user.Email),
    //             new Claim(ClaimTypes.Role, user.Role ?? "user")
    //         }),
    //         Expires = DateTime.UtcNow.Add(expiration),
    //         Issuer = issuer,
    //         Audience = audience,
    //         SigningCredentials = new SigningCredentials(
    //             new SymmetricSecurityKey(key),
    //             SecurityAlgorithms.HmacSha256
    //         )
    //     };

    //     var token = tokenHandler.CreateToken(tokenDescriptor);
    //     var tokenString = tokenHandler.WriteToken(token);

    //     // ===== Trả về response gọn gàng =====
    //     return Ok(new
    //     {
    //         code = 200,
    //         status = "success",
    //         message = "Đăng nhập thành công",
    //         data = new
    //         {
    //             token = tokenString,
    //             // role = user.Role
    //         }
    //     });
    // }

    // ==== Models nội bộ ====
    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class RegisterRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string DisplayName { get; set; }
    }

    public class ForgotPasswordRequest
    {
        public required string Email { get; set; }
    }

    public class GoogleUserRequest
    {
        public required string Email { get; set; }
        public required string Name { get; set; }
        public required string Avatar { get; set; }
    }

    public class GoogleLoginRequest
    {
        public required string Email { get; set; }
    }
}
