
using DrinksHubAPI.DataAccess;
using DrinksHubAPI.Model;
using Microsoft.AspNetCore.Identity;
using System.Runtime.CompilerServices;

namespace DrinksHubAPI.Data
{
	public class DemoData
	{
		private readonly IUserRepository _userRepository;

		public DemoData(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		internal static async Task InitializeAsync(IServiceProvider serviceProvider)
		{
			var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
			var demoData = new DemoData(userRepository);
			await demoData.SeedAdmin();
			await demoData.SeedUser();
		}

		public async Task SeedAdmin()
		{
			var seedAdmin = new User
			{
				Username = "seedAdmin",
				Role = "Admin"
			};

			var hasher = new PasswordHasher<User>();
			seedAdmin.PasswordHash = hasher.HashPassword(seedAdmin, "Admin123");

			await _userRepository.AddAsync(seedAdmin);
		}

		public async Task SeedUser()
		{
			var seedUser = new User
			{
				Username = "seedUser",
				Role = "User"
			};

			var hasher = new PasswordHasher<User>();
			seedUser.PasswordHash = hasher.HashPassword(seedUser, "User123");

			await _userRepository.AddAsync(seedUser);
		}
	}
}
