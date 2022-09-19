using ExpenseTracker.Common;
using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.User;
using ExpenseTracker.Services.Utils.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{

	public class AccountService : IAccountService
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly ITokenHandlerService tokenHandlerService;
		private readonly ExpenseTrackerDbContext db;

		public AccountService(UserManager<ApplicationUser> userManager
			,ITokenHandlerService tokenHandlerService,
			ExpenseTrackerDbContext db)
		{
			this.userManager = userManager;
			this.tokenHandlerService = tokenHandlerService;
			this.db = db;
		}

		public async Task<string> RegisterUser(UserRegisterModel model)
		{
			var userExists = await this.userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
			{
				throw new BadRequestException("Username is already taken");
			}

			var user = new ApplicationUser()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName,
			};

			var result = await this.userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				throw new BadRequestException("User creation failed!");
			}

			var isSucceeded = await userManager.AddToRoleAsync(user, RoleConstants.Client);

			if (!isSucceeded.Succeeded)
			{
				throw new BadRequestException("User creation failed!");
			}
			return "User created succesfully";
		}

		public async Task<AuthResponse> LoginUser(UserLoginModel model)
		{
			var user = await this.userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				throw new BadRequestException("Invalid credentials.");
			}
			var token = await this.tokenHandlerService.GenerateToken(user);
			return token;
		}

		public async Task<AuthResponse> RefreshToken(string userId, string refreshToken)
		{
			var user = await this.tokenHandlerService.GetIdFromToken(refreshToken);

			if (user == null)
			{
				throw new BadRequestException("Refresh token expired");
			}

			var dbUser = await this.db.Users.FirstOrDefaultAsync(x => x.Id == user);
			if (dbUser == null)
			{
				throw new InvalidOperationException("User not found");
			}

			return await this.tokenHandlerService.GenerateToken(dbUser);
		}

		public async Task<string> ChangePassword(string userId, string oldPassword, string newPassword)
		{
			var user = await this.userManager.FindByIdAsync(userId);

			if (user == null)
			{
				throw new BadRequestException("User not found");
			}

			var result = await userManager.ChangePasswordAsync(user, oldPassword, newPassword);

			if (result.Succeeded)
			{
				return "User password succesfully updated";
			}
			throw new BadRequestException("Unable to change user password");
		}

		public async  Task LogOut(string token)
		{
			await this.tokenHandlerService.RevokeAccessToken(token);
		}

		public async Task<UserResponse> GetCurrentUser(string id)
		{
			var user =await this.db.Users.FirstOrDefaultAsync(x => x.Id == id);

			if (user == null)
			{
				throw new BadRequestException("User not found");
			}
			// TODO: REFACTOR
			var role = await this.db.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.Id);
			var roleName = await this.db.Roles.FirstOrDefaultAsync(x => x.Id == role.RoleId);
			
			var response = new UserResponse(user);
			response.Role = roleName.Name;

			return response;

		}
	}
}
