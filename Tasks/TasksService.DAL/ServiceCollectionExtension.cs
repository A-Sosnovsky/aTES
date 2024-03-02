using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TasksService.DAL.Context;

namespace TasksService.DAL;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddDbContext<TasksDbContext>();
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        MigrateUp(services);
            
        return services;
    }

    private static void MigrateUp(IServiceCollection services)
    {
        using var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<TasksDbContext>();
        context.Database.Migrate();
    }
}