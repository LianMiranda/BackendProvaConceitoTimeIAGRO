using Microsoft.Extensions.DependencyInjection;

using Project.Domain.Interfaces;
using Project.Infrastructure.Repositories;

namespace Project.Infrastructure.Configs;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }
}