
using DrinksHubAPI.Data;
using DrinksHubAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;

namespace DrinksHubAPI
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			//Register repositories
			builder.Services.AddScoped<IDrinksRepository, DrinksRepository>();

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			builder.Services.AddDbContext<DrinksHubContext>(options => options.UseSqlServer(connectionString));

			var app = builder.Build();

			using (var scope = app.Services.CreateScope())
			{
				var context = scope.ServiceProvider.GetRequiredService<DrinksHubContext>();
				context.Database.Migrate();
				await DemoData.InitializeAsync(scope.ServiceProvider);
			}

			// Configure the HTTP request pipeline.
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
}
