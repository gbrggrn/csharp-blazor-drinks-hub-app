using DrinksHubAPI.Data;
using DrinksHubAPI.DataAccess;
using DrinksHubAPI.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

			//Add authentication
			var jwtSettings = builder.Configuration.GetSection("Jwt");
			var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidAudience = jwtSettings["Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
			});

			//Register repositories
			builder.Services.AddScoped<IDrinksRepository, DrinksRepository>();
			builder.Services.AddScoped<IUserRepository, UserRepository>();
			builder.Services.AddScoped<IReviewsRepository, ReviewsRepository>();
			builder.Services.AddScoped<IFavoritesRepository, FavoritesRepository>();

			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			builder.Services.AddDbContext<DrinksHubContext>(options =>
				options.UseSqlServer(connectionString));

			//Configure CORS
			builder.Services.AddCors(o =>
			{
				o.AddPolicy("default", p => p
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowAnyOrigin());
			});

			var app = builder.Build();

			//Seed database
			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				await DemoData.InitializeAsync(services);
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors("default");

			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
