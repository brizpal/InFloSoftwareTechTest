using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;
public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;

    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Get all users, optionally filtered by active state.
    /// </summary>
    public IEnumerable<User> GetUsers(bool? isActive = null)
    {
        var query = _dataAccess.GetAll<User>();

        if (isActive.HasValue)
        {
            query = query.Where(x => x.IsActive == isActive.Value);
        }

        return query.ToList();
    }

    public void AddUser(User user)
    {
        _dataAccess.Create(user);

        _dataAccess.LogAction(new UserAction
        {
            UserId = user.Id,
            ActionType = "Created",
            ActionDate = DateTime.UtcNow,
            PerformedBy = "System"
        });
    }

    public void UpdateUser(User user)
    {
        _dataAccess.Update(user);

        _dataAccess.LogAction(new UserAction
        {
            UserId = user.Id,
            ActionType = "Edited",
            ActionDate = DateTime.UtcNow,
            PerformedBy = "System"
        });
    }

    public void DeleteUser(User user)
    {
        _dataAccess.Delete(user);

        _dataAccess.LogAction(new UserAction
        {
            UserId = user.Id,
            ActionType = "Deleted",
            ActionDate = DateTime.UtcNow,
            PerformedBy = "System"
        });
    }

    public IEnumerable<UserAction> GetUserActions(int userId)
    {
        return _dataAccess.GetAll<UserAction>()
                          .Where(a => a.UserId == userId)
                          .OrderByDescending(a => a.ActionDate)
                          .ToList();
    }

    public IQueryable<UserAction> GetAllUserActions()
    {
        return _dataAccess.GetAll<UserAction>()
            .Include(a => a.User)
            .OrderByDescending(a => a.ActionDate);
    }
}

