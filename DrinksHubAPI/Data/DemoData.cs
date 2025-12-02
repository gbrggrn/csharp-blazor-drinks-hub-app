
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

		internal async Task InitializeAsync(IServiceProvider serviceProvider)
		{
			var hasher = new PasswordHasher<User>();

			var seedAdmin = new User
			{
				Username = "seedAdmin",
				Role = "Admin"
			};

			seedAdmin.PasswordHash = hasher.HashPassword(seedAdmin, "Admin123!");

			await _userRepository.AddAsync(seedAdmin);
		}
	}
}
