using DrinksHubAPI.DataAccess;
using DrinksHubAPI.Model;
using Microsoft.AspNetCore.Identity;

namespace DrinksHubAPI.Data
{
	public class DemoData
	{
		private readonly IUserRepository _userRepository;
		private readonly IDrinksRepository _drinksRepository;

		public DemoData(IUserRepository userRepository, IDrinksRepository drinksRepository)
		{
			_userRepository = userRepository;
			_drinksRepository = drinksRepository;
		}

		internal static async Task InitializeAsync(IServiceProvider serviceProvider)
		{
			var userRepository = serviceProvider.GetRequiredService<IUserRepository>();
			var drinksRepository = serviceProvider.GetRequiredService<IDrinksRepository>();
			var demoData = new DemoData(userRepository, drinksRepository);
			await demoData.SeedAdmin();
			await demoData.SeedUser();
			await demoData.SeedDrinks();
		}

		public async Task SeedAdmin()
		{
			if (_userRepository.GetAllQuery().Any())
			{
				return;
			}

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
			if (_userRepository.GetAllQuery().Count() > 1)
			{
				return;
			}

			var seedUser = new User
			{
				Username = "seedUser",
				Role = "User"
			};

			var hasher = new PasswordHasher<User>();
			seedUser.PasswordHash = hasher.HashPassword(seedUser, "User123");

			await _userRepository.AddAsync(seedUser);
		}

		public async Task SeedDrinks()
		{
			if (_drinksRepository.GetAllQuery().Any())
			{
				return;
			}

			List<Drink> seedDrinks = new();
			
			seedDrinks.Add(new Drink
			{
				Name = "Velvet Vanilla",
				Description = "A smooth fruit-forward red wine with notes of cherry and vanilla.",
				Category = "Red wine",
				Type = "Red",
				ImageUrl= "/images/drinks/drink1.png"
			});

			seedDrinks.Add(new Drink
			{
				Name = "Nexus",
				Description = "A crisp and refreshing sugar-free energy drink designed for clean focus.",
				Category = "Energy drink",
				Type = "Suger free",
				ImageUrl = "/images/drinks/drink2.png"
			});

			seedDrinks.Add(new Drink
			{
				Name = "Golden Dawn Latte",
				Description = "A mild and creamy coffee drink with subtle caramel sweetness.",
				Category = "Coffee",
				Type = "Latte",
				ImageUrl = "/images/drinks/drink3.png"
			});

			seedDrinks.Add(new Drink
			{
				Name = "Emerald Oolong",
				Description = "A lightly floral tea with a balanced, earthy finish.",
				Category = "Tea",
				Type = "Citrus",
				ImageUrl = "/images/drinks/drink4.png"
			});

			seedDrinks.Add(new Drink
			{
				Name = "Citrus Spark",
				Description = "A bright, zesty soda with natural lemon and lime flavors.",
				Category = "Soda",
				Type = "Citrus",
				ImageUrl = "/images/drinks/drink5.png"
			});

			foreach (var drink in seedDrinks)
			{
				await _drinksRepository.AddAsync(drink);
			}
		}
	}
}
