using Microsoft.Extensions.DependencyInjection;
using Project.Application.Services;

namespace Project.Application.Config;

public static class DependecyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();

        return services;
    }
}