using System;

namespace UserManagement.Web.Models.Users
{
    public class LogEntryViewModel
    {
        public int Id { get; set; }
        public string ActionType { get; set; } = string.Empty;
        public DateTime ActionDate { get; set; }
        public string PerformedBy { get; set; } = string.Empty;
        public string? TargetUser { get; set; }
    }

    public class LogsViewModel
    {
        public List<LogEntryViewModel> Entries { get; set; } = new();
    }
}
