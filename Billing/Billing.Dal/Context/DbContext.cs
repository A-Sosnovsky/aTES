using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Billing.DAL.Context;

public class BillingDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public BillingDbContext()
    {
        
    }
    
    public BillingDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public BillingDbContext(DbContextOptions<DbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_configuration?.GetConnectionString("BillingDb"));
        }

        optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
    }

}