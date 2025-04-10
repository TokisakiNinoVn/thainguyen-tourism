namespace BE.Controllers;

// Controllers/UserController.cs
using BE.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet("profile")]
    [Authorize(Roles = "user,admin")]
    public IActionResult GetProfile()
    {
        var username = User.Identity?.Name;
        var role = User.IsInRole("Admin") ? "Admin" : "User";

        return Ok(new
    {
        name = username,
        role = role
    });
    }
}
