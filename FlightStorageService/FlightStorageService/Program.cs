
using FlightStorageService.Caching;
using FlightStorageService.DAL;
using FlightStorageService.Dtos.MappingProfile;
using FlightStorageService.Services;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace FlightStorageService;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddAutoMapper(typeof(FlightProfile));
        builder.Services.AddControllersWithViews(opt =>
        {
            opt.Filters.Add<CustomExceptionFilterAttribute>();
        });
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ICacheHelper, CacheHelper>();
        builder.Services.AddLogging(configure => configure.AddConsole());
        builder.Services.AddDependecyForServices();
        builder.Services.AddDependecyForDatabase(builder.Configuration);
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gamestore api", Version = "v1" });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Gamestore api V1");
        });

        app.UseMiddleware<LoggingMiddleware>();
        app.MapControllers();

        app.Run();
    }
}
