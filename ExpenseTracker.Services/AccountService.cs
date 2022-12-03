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
			, ITokenHandlerService tokenHandlerService,
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
				Organization = new Organization()
				{
					Name = model.Organization,
					Currency = model.Currency.Currency,
					Abbreviation = model.Currency.Abbreviation,
					CurrencySymbol = model.Currency.Symbol
				},

				Settings = new Settings()
				{
					Mode = "light"
				},

			};

			var result = await this.userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				throw new BadRequestException("User creation failed!");
			}


			var isSucceeded = await userManager.AddToRoleAsync(user, RoleConstants.Client);
			var isOwner = await userManager.AddToRoleAsync(user, RoleConstants.Owner);

			if (!isSucceeded.Succeeded || !isOwner.Succeeded)
			{
				throw new BadRequestException("User creation failed!");
			}

			user.Organization.Owner = user.Id;
			await this.db.SaveChangesAsync();

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

		public async Task LogOut(string token)
		{
			await this.tokenHandlerService.RevokeAccessToken(token);
		}

		public async Task<UserResponse> GetCurrentUser(string id)
		{
			var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == id);

			if (user == null)
			{
				throw new BadRequestException("User not found");
			}

			var userRoles = await this.db.UserRoles.Where(x => x.UserId == user.Id).ToListAsync();
			var roleNames = new List<string>();
			foreach (var userRole in userRoles)
			{
				var currRole =  await this.db.Roles.FirstOrDefaultAsync(x => x.Id == userRole.RoleId);

				if (currRole is not null)
				{
					roleNames.Add(currRole.Name);
				}
			}

			var response = new UserResponse(user);
			response.Roles = roleNames.ToList();

			return response;

		}

		public async Task<string> UpdateUser(UserUpdateModel model)
		{
			var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);

			if (user == null)
			{
				throw new BadRequestException("User not found");
			}

			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.UserName = model.UserName;
			user.Email = model.Email;
			user.PhoneNumber = model.PhoneNumber;

			this.db.Users.Update(user);
			var result = await this.db.SaveChangesAsync();

			if (result == 1)
			{
				return "User updated succesfully.";
			}

			return "Unable to update. Please try again later. If the problem persists contact with your administrator.";
		}

		public async  Task AddUser(RegisterEmployeeModel model, Organization organization)
		{
			var userExists = await this.userManager.FindByEmailAsync(model.Email);

			if (userExists != null)
			{
				throw new BadRequestException("Unable to create employee.");
			}

			var user = new ApplicationUser()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Organization = organization,
				OrganizationId = organization.Id,
				Settings = new Settings()
				{
					Mode="light"
				}
			};

			var result = await this.userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded)
			{
				throw new BadRequestException("Unable to create employee.");
			}
			 await userManager.AddToRoleAsync(user, RoleConstants.Employee);
			 await userManager.AddToRoleAsync(user, RoleConstants.Client);

		}

		public async Task ChangeMode(string mode,string userId)
		{
			var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == userId);

			if (user is null)
			{
				throw new BadRequestException("Invalid user");
			}
			user.Settings.Mode = mode;
			await this.db.SaveChangesAsync();

		}

		public async Task<IEnumerable<ApplicationUser>> GetAllUsers()
		{
			return this.db.Users.ToList();
		}

		public async Task ConfirmUserEmail(string userId)
		{
			var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == userId);

			if(userId is null)
			{
				throw new BadRequestException("Invalid user");
			}

			user.EmailConfirmed = true;
			await this.db.SaveChangesAsync();

		}
	}
}
