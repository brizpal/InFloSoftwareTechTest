using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public interface IDataContext
{

    /// <summary>
    /// Get all entities of type <typeparamref name="TEntity"/>.
    /// </summary>
    IQueryable<TEntity> GetAll<TEntity>() where TEntity : class;

    /// <summary>
    /// Create a new entity.
    /// </summary>
    void Create<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Update an existing entity.
    /// </summary>
    void Update<TEntity>(TEntity entity) where TEntity : class;

    /// <summary>
    /// Delete an entity.
    /// </summary>
    void Delete<TEntity>(TEntity entity) where TEntity : class;

    
    /// <summary>
    /// User action logs (DbSet).
    /// </summary>
    DbSet<UserAction> UserActions { get; set; }

    /// <summary>
    /// Record a user action (e.g. Created, Edited, Deleted).
    /// </summary>
    void LogAction(UserAction action);
}
