using DrinksHubApp.Client.Pages;
using DrinksHubApp.Components;
using DrinksHubApp.Services;

namespace DrinksHubApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            // Add scoped services
            builder.Services.AddScoped<TokenStore>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<DrinksHubApiService>();

            // Add Http client to services that use it
            builder.Services.AddHttpClient<AuthService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7080/");
            });
            builder.Services.AddHttpClient<DrinksHubApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7080/");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }
}
