using Billing.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.DAL;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDal(this IServiceCollection services)
    {
        services.AddDbContext<BillingDbContext>();
        services.AddScoped<IRepository, Repository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        MigrateUp(services);
            
        return services;
    }

    private static void MigrateUp(IServiceCollection services)
    {
        using var serviceScope = services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<BillingDbContext>();
        context.Database.Migrate();
    }
}