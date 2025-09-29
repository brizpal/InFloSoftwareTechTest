using System.Linq;
using Microsoft.EntityFrameworkCore;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

public class LogsController : Controller
{
    private readonly IUserService _userService;

    public LogsController(IUserService userService) => _userService = userService;

    
    [HttpGet]
    public IActionResult Index()
    {
        var logs = _userService.GetAllUserActions() 
    .Include(log => log.User)
    .OrderByDescending(log => log.ActionDate)
    .Select(log => new LogEntryViewModel
    {
        Id = log.Id,
        ActionType = log.ActionType,
        ActionDate = log.ActionDate,
        PerformedBy = string.IsNullOrEmpty(log.PerformedBy) ? "System" : log.PerformedBy,
        TargetUser = log.User != null
                     ? log.User.Forename + " " + log.User.Surname
                     : null
    })
    .ToList();


        var model = new LogsViewModel { Entries = logs };
        return View(model);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var log = _userService.GetAllUserActions() 
            .Include(log => log.User)
            .FirstOrDefault(log => log.Id == id);

        if (log == null) return NotFound();

        var model = new LogEntryViewModel
        {
            Id = log.Id,
            ActionType = log.ActionType,
            ActionDate = log.ActionDate,
            PerformedBy = string.IsNullOrEmpty(log.PerformedBy) ? "System" : log.PerformedBy,
            TargetUser = log.User != null ? $"{log.User.Forename} {log.User.Surname}" : null
        };

        return View(model);
    }
}
