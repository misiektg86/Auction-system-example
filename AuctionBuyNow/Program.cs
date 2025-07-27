
using AuctionBuyNow.Application;
using AuctionBuyNow.Infrastructure;
using Microsoft.OpenApi.Models;

namespace AuctionBuyNow.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Auction API",
                Version = "v1"
            });
        });

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}