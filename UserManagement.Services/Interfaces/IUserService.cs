using System.Collections.Generic;
using System.Linq;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Get all users, optionally filtered by active state.
    /// </summary>
    IEnumerable<User> GetUsers(bool? isActive = null);

    /// <summary>
    /// Add a new user.
    /// </summary>
    void AddUser(User user);

    /// <summary>
    /// Update an existing user.
    /// </summary>
    void UpdateUser(User user);

    /// <summary>
    /// Delete an existing user.
    /// </summary>
    void DeleteUser(User user);

    /// <summary>
    /// Get all actions performed for a given user.
    /// </summary>
    IEnumerable<UserAction> GetUserActions(int userId);

    IQueryable<UserAction> GetAllUserActions();
}

