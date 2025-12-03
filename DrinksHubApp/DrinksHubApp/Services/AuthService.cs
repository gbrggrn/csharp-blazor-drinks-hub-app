using DrinksHubApp.DTOs;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;

namespace DrinksHubApp.Services
{
	public class AuthService
	{
		private readonly HttpClient _http;
		private readonly TokenStore _tokenStore;

		public AuthService(HttpClient http, TokenStore tokenStore)
		{
			_http = http;
			_http.BaseAddress = new Uri("https://localhost:5119");
			_tokenStore = tokenStore;
		}

		[HttpPost("api/Auth/login")]
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

			var tokenString = await response.Content.ReadFromJsonAsync<AccessTokenResponse>();
			if (tokenString == null || string.IsNullOrEmpty(tokenString.AccessToken))
			{
				return false;
			}

			_tokenStore.SetToken(tokenString.AccessToken);

			return true;
		}
	}
}
