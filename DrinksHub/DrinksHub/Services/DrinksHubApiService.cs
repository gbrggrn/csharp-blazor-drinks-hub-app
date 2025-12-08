using DrinksHub.DTOs;
using System.Net.Http.Headers;
using System.Text;

namespace DrinksHub.Services
{
	public class DrinksHubApiService
	{
		private readonly HttpClient _http;
		private readonly TokenStore _tokenStore;

		public DrinksHubApiService(HttpClient http, TokenStore tokenStore)
		{
			_http = http;
			_tokenStore = tokenStore;
		}

		private void AttachTokenToRequest()
		{
			if (!string.IsNullOrEmpty(_tokenStore.Token))
			{
				_http.DefaultRequestHeaders.Authorization =
					new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
			}
		}

		public async Task<List<ResponseDrinkDTO>> GetAllDrinksAsync(
			DrinkSortOption? sortOption = null, 
			DrinkCategory? filterCategory = null,
			DrinkType? filterType = null,
			string? searchParam = null)
		{
			var builtQuery = new List<string>();

			if (!string.IsNullOrEmpty(searchParam))
				builtQuery.Add($"search={Uri.EscapeDataString(searchParam)}");

			if (sortOption.HasValue)
				builtQuery.Add($"sortBy={sortOption.Value}");

			if (filterCategory.HasValue)
				builtQuery.Add($"filterCategory={filterCategory.Value}");

			if (filterType.HasValue)
				builtQuery.Add($"filterType={filterType.Value}");

			var queryString = builtQuery.Count > 0 ? "?" + string.Join("&", builtQuery) : "";

			var drinks = await _http.GetFromJsonAsync<List<ResponseDrinkDTO>>($"api/Drinks{queryString}");
			return drinks ?? new List<ResponseDrinkDTO>();
		}

		public async Task<ResponseDrinkDTO?> GetDrinkByIdAsync(int idIn)
		{
			var drink = await _http.GetFromJsonAsync<ResponseDrinkDTO>($"api/Drinks/{idIn}");
			return drink;
		}

		public async Task<bool> CreateDrinkAsync(CreateDrinkDTO drinkDtoIn)
		{
			AttachTokenToRequest();

			var response = await _http.PostAsJsonAsync("api/Drinks", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> UpdateDrinkAsync(int idIn, UpdateDrinkDTO drinkDtoIn)
		{
			AttachTokenToRequest();

			var response = await _http.PutAsJsonAsync($"api/Drinks/{idIn}", drinkDtoIn);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> DeleteDrinksAsync(int idIn)
		{
			AttachTokenToRequest();

			var response = await _http.DeleteAsync($"api/Drinks/{idIn}");

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<List<ResponseDrinkDTO>> GetFavoritesAsync(int userId)
		{
			AttachTokenToRequest();

			var favorites = await _http.GetFromJsonAsync<List<ResponseDrinkDTO>>($"api/Favorites/{userId}");
				
			return favorites ?? new List<ResponseDrinkDTO>();
		}

		public async Task<bool> AddToFavoritesAsync(int drinkId, int userId)
		{
			AttachTokenToRequest();

			var response = await _http.PostAsync($"api/Favorites/{drinkId}?userId={userId}", null);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}
		
		public async Task<bool> RemoveFromFavoritesAsync(int drinkId, int userId)
		{
			AttachTokenToRequest();

			var response = await _http.DeleteAsync($"api/Favorites/{drinkId}?userId={userId}");

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}

		public async Task<bool> AddReviewAsync(int drinkId, CreateReviewDTO reviewDto)
		{
			AttachTokenToRequest();

			var response = await _http.PostAsJsonAsync($"api/Drinks/{drinkId}/reviews", reviewDto);

			return response.IsSuccessStatusCode;
		}
	}
}