using UserManagement.Core.DTOs;

namespace UserManagement.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<(bool Success, string Message, UserDto? User)> CreateUserAsync(UserDto dto);
        Task<(bool Success, string Message)> UpdateUserAsync(int id, UserDto dto);
        Task<(bool Success, string Message)> DeleteUserAsync(int id);
    }
}
