using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Data.Repositories
{
    public class UserRepository
    {
        private readonly List<User> _users = new()
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com",DateOfBirth = new DateOnly(2000, 2, 10), IsActive = true },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com",DateOfBirth = new DateOnly(2001, 2, 10),  IsActive = true },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com",DateOfBirth = new DateOnly(2002, 2, 10),  IsActive = false },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com",DateOfBirth = new DateOnly(2003, 3, 10),  IsActive = true },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com",DateOfBirth = new DateOnly(2004, 2, 10),  IsActive = true },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com",DateOfBirth = new DateOnly(2005, 4, 8),  IsActive = true },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com",DateOfBirth = new DateOnly(2007, 2, 10),  IsActive = false },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com",DateOfBirth = new DateOnly(2006, 2, 10),  IsActive = false },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com",DateOfBirth = new DateOnly(1998, 7, 8),  IsActive = false },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com",DateOfBirth = new DateOnly(1999, 8, 9),  IsActive = true },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com",DateOfBirth = new DateOnly(1997, 4, 6),  IsActive = true },
        };

        public Task<List<User>> GetAllAsync() => Task.FromResult(_users);

        public Task<User?> GetByIdAsync(int id) =>
            Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

        public Task AddAsync(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            _users.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Forename = user.Forename;
                existing.Surname = user.Surname;
                existing.Email = user.Email;
                existing.DateOfBirth = user.DateOfBirth;
                existing.IsActive = user.IsActive;
            }
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null) _users.Remove(user);
            return Task.CompletedTask;
        }
    }
}
