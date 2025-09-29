using System.Linq;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.WebMS.Controllers;

public class UsersController : Controller
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    // LIST
    [HttpGet]
    public IActionResult Index(bool? isActive = null)
    {
        var users = _userService.GetUsers(isActive)
            .Select(MapToViewModel)
            .ToList();

        return View("List", new UserListViewModel { Items = users });
    }

    // CREATE
    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(UserViewModel model)
    {
        if (!ValidateUser(model))
            return View(model);

        _userService.AddUser(MapToEntity(model));
        return RedirectToAction(nameof(Index));
    }

    // DETAILS
    [HttpGet]
    public IActionResult Details(int id)
    {
        var user = _userService.GetUsers().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        var actions = _userService.GetUserActions(id)
           .Select(a => new Web.Models.Users.UserAction
           {
               ActionType = a.ActionType,
               ActionDate = a.ActionDate,
               PerformedBy = a.PerformedBy
           })
           .ToList();

        var model = new UserDetailsViewModel
        {
            User = MapToViewModel(user),
            Actions = actions
        };

        return View(model);
    }

    // EDIT
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var user = _userService.GetUsers().FirstOrDefault(u => u.Id == id);
        return user == null ? NotFound() : View(MapToViewModel(user));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(UserViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var existingUser = _userService.GetUsers().FirstOrDefault(u => u.Id == model.Id);
        if (existingUser == null) return NotFound();

        UpdateEntity(existingUser, model);
        _userService.UpdateUser(existingUser);

        return RedirectToAction(nameof(Index));
    }

    // DELETE
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var user = _userService.GetUsers().FirstOrDefault(u => u.Id == id);
        return user == null ? NotFound() : View(MapToViewModel(user));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var user = _userService.GetUsers().FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        _userService.DeleteUser(user);
        return RedirectToAction(nameof(Index));
    }

    // === PRIVATE HELPERS ===

    private static bool ValidateUser(UserViewModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Forename))
            return false;
        if (string.IsNullOrWhiteSpace(model.Surname))
            return false;

        return true;
    }

    private static void UpdateEntity(User entity, UserViewModel model)
    {
        entity.Forename = model.Forename?.Trim() ?? string.Empty;
        entity.Surname = model.Surname?.Trim() ?? string.Empty;
        entity.Email = model.Email?.Trim() ?? string.Empty;
        entity.DateOfBirth = model.DateOfBirth;
        entity.IsActive = model.IsActive;
    }

    private static User MapToEntity(UserViewModel model) => new User
    {
        Forename = model.Forename?.Trim() ?? string.Empty,
        Surname = model.Surname?.Trim() ?? string.Empty,
        Email = model.Email?.Trim() ?? string.Empty,
        DateOfBirth = model.DateOfBirth,
        IsActive = model.IsActive
    };

    private static UserViewModel MapToViewModel(User user) => new UserViewModel
    {
        Id = user.Id,
        Forename = user.Forename,
        Surname = user.Surname,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        IsActive = user.IsActive
    };
}


