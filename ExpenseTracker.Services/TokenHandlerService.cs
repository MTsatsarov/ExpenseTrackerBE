using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Config;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.Services
{
	public class TokenHandlerService:ITokenHandlerService
	{
		private readonly long expirationInSeconds = 1 * 60 * 60;
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ExpenseTrackerDbContext db;
		private readonly IDistributedCache cache;
		private readonly TokenConfig tokenConfig;

		public TokenHandlerService(UserManager<ApplicationUser> userManager,
			ExpenseTrackerDbContext db, IOptions<TokenConfig> tokenConfig
			,IDistributedCache cache)
		{
			this.userManager = userManager;
			this.db = db;
			this.cache = cache;
			this.tokenConfig = tokenConfig.Value;
		}
		public async Task<AuthResponse> GenerateToken(ApplicationUser user)
		{
			DateTime tokenExpiration = DateTime.UtcNow.AddSeconds(expirationInSeconds);

			var userRoles = await this.userManager.GetRolesAsync(user);
			//TODO Remove these request to db and get it without db request
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier,user.Id),
				new Claim(ClaimTypes.Email,user.Email),
				new Claim(ClaimTypes.Name,user.UserName),
				new Claim(ClaimTypes.Role,userRoles.FirstOrDefault()),
			};

			foreach (var role in userRoles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role.ToUpper()));
			}

			var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Key));

			var securityToken = new JwtSecurityToken(
				issuer: tokenConfig.Issuer,
				audience: tokenConfig.Audience,
				expires: tokenExpiration,
				claims: claims,
				signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

			var handler = new JwtSecurityTokenHandler();
			string token = handler.WriteToken(securityToken);
			await this.SetStringInCache(token, user.Id.ToString(), 1 * 60 * 60);

			var refreshToken = this.RefreshToken();
			await this.SetStringInCache(refreshToken, user.Id.ToString(), 2 * 60 * 60);

			this.db.Users.Update(user);
			await this.db.SaveChangesAsync();

			return new AuthResponse()
			{
				Token = token,
				RefreshToken = refreshToken
			};
		}

		public async Task<string> GetIdFromToken(string refreshToken)
		{
			if (string.IsNullOrEmpty(refreshToken))
			{
				return null;
			}

			var id = await cache.GetStringAsync(refreshToken);
			return id;
		}

		private string RefreshToken()
		{
			var randomNumber = new byte[64];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}

		private async Task SetStringInCache(string token, string id, long expirationTime)
		{
			var distributedCacheEntry = new DistributedCacheEntryOptions()
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationTime)
			};
			await cache.SetStringAsync(token, id, distributedCacheEntry);
		}
		public async Task RevokeAccessToken(string refreshToken)
		{
			await this.cache.RemoveAsync(refreshToken);
		}
	}
}
