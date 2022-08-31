using ExpenseTracker.Services.Models;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IAccountService
	{
		 Task<string> RegisterUser(UserRegisterModel model);

		 Task<AuthResponse> LoginUser(UserLoginModel model);

		Task<AuthResponse> RefreshToken(string userId, string refreshToken);

		Task LogOut(string token);

		Task<string> ChangePassword(string userId, string oldPassword, string newPassword);
	}
}
