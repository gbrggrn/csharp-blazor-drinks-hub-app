using DrinksHubAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrinksHubAPI.Helpers
{
	public static class JwtTokenHelper
	{
		public static string JwtTokenProvider(User user, IConfiguration config)
		{
			var jwtSettings = config.GetSection("Jwt");
			var keyString = jwtSettings["Key"];
			if (string.IsNullOrWhiteSpace(keyString))
			{
				throw new InvalidOperationException("No JWT config for KEY.");
			}

			var key = Encoding.UTF8.GetBytes(keyString);

			var durationSetting = jwtSettings["DurationInMinutes"] ?? jwtSettings["ExpiresMinutes"];
			if (!int.TryParse(durationSetting, out var durationMinutes))
			{
				durationMinutes = 60; //Fallback value
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.Role, user.Role)
			};

			var creds = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(durationMinutes),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
