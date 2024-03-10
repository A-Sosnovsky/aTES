using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Billing.DAL.Context;

public class BillingDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Account> Accounts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;
    public DbSet<BillingCycle> BillingCycles { get; set; } = null!;
    public DbSet<Task> Tasks { get; set; } = null!;
    
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BillingCycle>()
            .HasIndex(bc => bc.Status)
            .HasFilter($"{nameof(BillingCycle.Status)} = '{(int)BillingCycleStatus.Active}'")
            .IsUnique();
    }
}