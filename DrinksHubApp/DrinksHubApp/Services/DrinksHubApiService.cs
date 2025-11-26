
using DrinksHubApp.DTOs;

namespace DrinksHubApp.Services
{
	public class DrinksHubApiService
	{
		HttpClient _http;

		public DrinksHubApiService(HttpClient http)
		{
			_http = http;
			_http.BaseAddress = new Uri("https://localhost:5119");
        }

		public async Task<List<DrinkDto>> GetAllDrinksAsync()
		{
			var drinks = await _http.GetFromJsonAsync<List<DrinkDto>>("api/Drinks");

			return drinks ?? new List<DrinkDto>();
		}

		public async Task<DrinkDto?> GetDrinkByIdAsync(int idIn)
		{
			var drink = await _http.GetFromJsonAsync<DrinkDto>($"api/Drinks/{idIn}");
			return drink;
		}

		public async Task<bool> CreateDrinkAsync(DrinkDto drinkDtoIn)
		{
			var response = await _http.PostAsJsonAsync("api/Drinks", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> UpdateDrinkAsync(int idIn, DrinkDto drinkDtoIn)
		{
			var response = await _http.PutAsJsonAsync($"api/Drinks/{idIn}", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> DeleteDrinksAsync(int idIn)
		{
			var response = await _http.DeleteAsync($"api/Drinks/{idIn}");

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}
    }
}