using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.User;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IAccountService
	{
		Task<string> RegisterUser(UserRegisterModel model);

		Task AddUser(RegisterEmployeeModel model, Organization organization);

		Task<AuthResponse> LoginUser(UserLoginModel model);

		Task<AuthResponse> RefreshToken(string userId, string refreshToken);

		Task LogOut(string token);

		Task<string> ChangePassword(string userId, string oldPassword, string newPassword);

		Task<UserResponse> GetCurrentUser(string id);

		Task<string> UpdateUser(UserUpdateModel model);
	}
}
