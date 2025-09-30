using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.Interfaces;
using UserManagement.Data.Repositories;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(ToDto).ToList();
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : ToDto(user);
        }

        public async Task<(bool Success, string Message, UserDto? User)> CreateUserAsync(UserDto dto)
        {
            if (dto == null)
                return (false, "User data is required.", null);

            var existingUsers = await _repository.GetAllAsync();
            if (existingUsers.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "A user with this email already exists.", null);
            }

            var user = FromDto(dto);
            await _repository.AddAsync(user);

            return (true, "User created successfully.", ToDto(user));
        }

        public async Task<(bool Success, string Message)> UpdateUserAsync(int id, UserDto dto)
        {
            if (dto == null)
                return (false, "User data is required.");

            if (id != dto.Id)
                return (false, "User ID mismatch.");

            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return (false, "User not found.");

            var allUsers = await _repository.GetAllAsync();
            if (allUsers.Any(u => u.Id != id && u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                return (false, "Another user with this email already exists.");

            var user = FromDto(dto);
            await _repository.UpdateAsync(user);

            return (true, "User updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            var existingUser = await _repository.GetByIdAsync(id);
            if (existingUser == null)
                return (false, "User not found.");

            await _repository.DeleteAsync(id);
            return (true, "User deleted successfully.");
        }

        // --- Mapping Helpers ---
        private static UserDto ToDto(User user) =>
            new UserDto
            {
                Id = (int)user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };

        private static User FromDto(UserDto dto) =>
            new User
            {
                Id = dto.Id,
                Forename = dto.Forename,
                Surname = dto.Surname,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                IsActive = dto.IsActive
            };
    }
}
