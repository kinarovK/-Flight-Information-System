using FlightClientApp.ApiClient;
using FlightClientApp.Services;

namespace FlightClientApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var flightApiBaseUrl = builder.Configuration["FlightApi:BaseUrl"];
        if (string.IsNullOrEmpty(flightApiBaseUrl))
        {
            throw new InvalidOperationException("Flight API base URL is not configured.");
        }
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddScoped<IFlightApiService, FlightApiService>();
        builder.Services.AddHttpClient<IFlightApiClient, FlightApiClient>(client =>
        {
            client.BaseAddress = new Uri(flightApiBaseUrl);
        });

        var app = builder.Build();
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
