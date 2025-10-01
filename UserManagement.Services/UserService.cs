using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Core.DTOs;
using UserManagement.Core.Interfaces;

namespace UserManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // --- Get all users ---
        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _repository.GetAllAsync() ?? new List<User>();
            return users.Select(ToDto).ToList();
        }

        // --- Get user by ID ---
        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user == null ? null : ToDto(user);
        }

        // --- Create user ---
        public async Task<(bool Success, string Message, UserDto? User)> CreateUserAsync(UserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var existing = await _repository.GetAllAsync() ?? new List<User>();
            if (existing.Any(u => u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase)))
                return (false, "A user with this email already exists.", null);

            var user = FromDto(dto);
            await _repository.AddAsync(user);
            return (true, "User created successfully.", ToDto(user));
        }

        // --- Update user ---
        public async Task<(bool Success, string Message)> UpdateUserAsync(int id, UserDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            if (id != dto.Id)
                return (false, "User ID mismatch.");

            await _repository.UpdateAsync(FromDto(dto));
            return (true, "User updated successfully.");
        }

        // --- Delete user ---
        public async Task<(bool Success, string Message)> DeleteUserAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            if (user == null)
                return (false, "User not found.");

            await _repository.DeleteAsync(id);
            return (true, "User deleted successfully.");
        }

        // --- Mapping helpers ---
        private static UserDto ToDto(User? user)
        {
            if (user == null) return null!;

            return new UserDto
            {
                Id = (int)user.Id,
                Forename = user.Forename,
                Surname = user.Surname,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                IsActive = user.IsActive
            };
        }

        private static User FromDto(UserDto dto)
        {
            return new User
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
}
