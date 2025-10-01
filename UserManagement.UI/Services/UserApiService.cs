using System.Net.Http.Json;
using UserManagement.UI.Models;

namespace UserManagement.UI.Services;

public class UserApiService
{
    private readonly HttpClient _http;

    public UserApiService(HttpClient http) => _http = http;

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _http.GetFromJsonAsync<List<UserDto>>("api/users");
        return users ?? new List<UserDto>();
    }


    public Task<UserDto?> GetUserByIdAsync(int id) =>
        _http.GetFromJsonAsync<UserDto>($"api/users/{id}");

    public Task<HttpResponseMessage> CreateUserAsync(UserDto user) =>
        _http.PostAsJsonAsync("api/users", user);

    public Task<HttpResponseMessage> UpdateUserAsync(int id, UserDto user) =>
        _http.PutAsJsonAsync($"api/users/{id}", user);

    public Task<HttpResponseMessage> DeleteUserAsync(int id) =>
        _http.DeleteAsync($"api/users/{id}");
}
