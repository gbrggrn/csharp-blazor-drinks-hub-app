using DrinksHubAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DrinksHubAPI.Helpers
{
	public static class JwtTokenHeper
	{
		public static string JwtTokenProvider(User user, IConfiguration config)
		{
			var jwtSettings = config.GetSection("Jwt");
			var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

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
				expires: DateTime.Now.AddMinutes(int.Parse(jwtSettings["DurationInMinutes"]!)),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
