namespace UserManagement.Web.Models.Users;

public class UserDetailsViewModel
{
    public UserViewModel User { get; set; } = default!;
    public List<UserAction> Actions { get; set; } = new();
}
