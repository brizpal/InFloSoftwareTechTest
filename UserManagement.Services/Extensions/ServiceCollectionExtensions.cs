using Microsoft.Extensions.DependencyInjection;
using UserManagement.Services;
using UserManagement.Core.Interfaces;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
        => services.AddScoped<IUserService, UserService>();
}
