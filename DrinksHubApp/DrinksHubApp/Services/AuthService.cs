using DrinksHubApp.DTOs;
using static System.Net.WebRequestMethods;

namespace DrinksHubApp.Services
{
	public class AuthService
	{
		HttpClient _http;

		public AuthService(HttpClient http)
		{
			_http = http;
			_http.BaseAddress = new Uri("https://localhost:5119");
		}

		public async Task<bool> LoginAsync(LoginRequestDTO loginRequestDTO)
		{
			if (loginRequestDTO == null)
			{
				return false;
			}

			var response = await _http.PostAsJsonAsync("api/Auth/login", loginRequestDTO);

			if (!response.IsSuccessStatusCode)
			{
				return false;
			}

			return true;
		}
	}
}
