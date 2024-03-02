using System.Threading.Tasks;
using KafkaFlow;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TasksService.DAL;
using TasksService.Services;

namespace TasksService.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddDal()
            .AddEventsService()
            .AddControllers();
            
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        await app.Services.CreateKafkaBus().StartAsync();
        await app.RunAsync();
    }
}