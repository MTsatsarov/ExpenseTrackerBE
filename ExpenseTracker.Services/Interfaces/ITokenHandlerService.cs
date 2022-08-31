using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Models;

namespace ExpenseTracker.Services.Interfaces
{
	public interface ITokenHandlerService
	{
		Task<AuthResponse> GenerateToken(ApplicationUser user);

		Task<string> GetIdFromToken(string refreshToken);

		Task RevokeAccessToken(string refreshToken);
	}
}
