using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TasksService.DAL.Context;

public class TasksDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public TasksDbContext()
    {
        
    }
    
    public TasksDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TasksDbContext(DbContextOptions<DbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_configuration?.GetConnectionString("TasksDb"));
        }

        optionsBuilder.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name });
    }

    public DbSet<Task> Tasks { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}