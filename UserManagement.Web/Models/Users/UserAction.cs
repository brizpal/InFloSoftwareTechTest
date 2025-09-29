using System;

namespace UserManagement.Web.Models.Users;

public class UserAction
{
    public int Id { get; set; }
    public long UserId { get; set; }
    public required string ActionType { get; set; }
    public DateTime ActionDate { get; set; }
    public string? PerformedBy { get; set; }
}
