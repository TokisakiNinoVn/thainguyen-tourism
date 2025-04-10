using BE;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/admin/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    // DELETE: api/user/{id}
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy người dùng"
            });
        }

        _context.Users.Remove(user);
        _context.SaveChanges();

        return Ok(new
        {
            message = "Xóa người dùng thành công",
            userId = id
        });
    }
}
