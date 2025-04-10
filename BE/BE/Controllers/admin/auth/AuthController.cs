//file: AuthController.cs
namespace BE.Controllers.admin.auth;

using BE;
using BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    // [Authorize(Roles = "admin")]
    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Tài khoản và mật khẩu không được để trống",
                data = (object?)null
            });
        }

        var existing = _context.Users.FirstOrDefault(u => u.Email == user.Email);

        // ✅ Kiểm tra xem tài khoản có tồn tại không
        if (existing == null)
        {
            return BadRequest(new
            {
                code = 400,
                status = "error",
                message = "Tài khoản không tồn tại",
                data = (object?)null
            });
        }

        // ✅ So sánh mật khẩu đã mã hoá bằng BCrypt
        if (!BCrypt.Net.BCrypt.Verify(user.Password, existing.Password))
        {
            return Unauthorized(new
            {
                code = 401,
                status = "error",
                message = "Sai mật khẩu",
                data = (object?)null
            });
        }

        // ✅ Kiểm tra role của tài khoản
        if (existing.Role != "admin")
        {
            return Unauthorized(new
            {
                code = 403,
                status = "error",
                message = "Bạn không có quyền truy cập",
                data = (object?)null
            });
        }

        // ✅ Tạo JWT token
        var token = GenerateJwtToken(existing);
        return Ok(new
        {
            code = 200,
            status = "success",
            message = "Đăng nhập thành công",
            data = new
            {
                role = existing.Role,
                token
            }
        });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        var jwtExpiration = Environment.GetEnvironmentVariable("JWT_EXPIRATION");

        if (string.IsNullOrWhiteSpace(jwtKey) || string.IsNullOrWhiteSpace(jwtIssuer) ||
            string.IsNullOrWhiteSpace(jwtAudience) || string.IsNullOrWhiteSpace(jwtExpiration))
        {
            throw new Exception("JWT cấu hình bị thiếu trong .env");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(ParseExpiration(jwtExpiration)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private double ParseExpiration(string expiration)
    {
        // Hỗ trợ: "1h", "30m", "2d"
        if (expiration.EndsWith("h"))
        {
            return double.Parse(expiration.Replace("h", ""));
        }
        else if (expiration.EndsWith("m"))
        {
            return double.Parse(expiration.Replace("m", "")) / 60.0;
        }
        else if (expiration.EndsWith("d"))
        {
            return double.Parse(expiration.Replace("d", "")) * 24.0;
        }
        else
        {
            // Mặc định 1 giờ nếu định dạng sai
            return 1.0;
        }
    }
}
