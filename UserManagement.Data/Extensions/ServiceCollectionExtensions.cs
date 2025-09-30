using Microsoft.Extensions.DependencyInjection;
using UserManagement.Data.Repositories;

namespace UserManagement.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
        => services.AddSingleton<UserRepository>();
}
