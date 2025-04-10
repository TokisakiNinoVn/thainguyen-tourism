using BE.Models;
using System.Threading.Tasks;

namespace BE.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
    }
}
