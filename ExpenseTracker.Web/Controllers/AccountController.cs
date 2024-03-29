﻿using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly IAccountService accountService;

		public AccountController(IAccountService accountService)
		{
			this.accountService = accountService;
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("signUp")]
		public async Task<IActionResult> Register(UserRegisterModel model)
		{
			var result = string.Empty;
			try
			{
				result = await this.accountService.RegisterUser(model);

			}
			catch (Exception e)
			{
				return this.BadRequest(e.Message);
			}

			return this.Ok(result);
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("signIn")]
		public async Task<IActionResult> Login(UserLoginModel model)
		{
			var result = await this.accountService.LoginUser(model);

			return this.Ok(result);
		}

		[HttpPost]
		[Route("logOut")]
		public async Task<IActionResult> LogOut()
		{
			try
			{
				if (!User.Claims.Any())
				{
					return this.Ok();
				}

				var token = HttpContext.Request.Headers["Authorization"]
					.ToString().Replace("Bearer", string.Empty);

				await accountService.LogOut(token);
			}
			catch (Exception e)
			{

				return this.BadRequest("Unable to remove token");
			}

			return this.Ok();
		}

		[HttpPost]
		[Route("changePassword")]
		[Authorize(Roles = "CLIENT")]
		public async Task<IActionResult> ChangePass([FromBody] PasswordChangeModel model)
		{
			try
			{
				var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
				var result = await this.accountService.ChangePassword(userId, model.OldPassword, model.NewPassword);
				return this.Ok(result);
			}
			catch (Exception e)
			{

				return this.BadRequest("Unable to change user password");
			}
			return this.BadRequest("Unable to change user password");
		}


		[HttpPost]
		[Route("refreshToken")]
		[AllowAnonymous]
		public async Task<IActionResult> RefreshToken(RefreshRequest request)
		{
			if (request.RefreshToken == null || request.UserId == null)
			{
				return this.BadRequest();
			}
			var authResponse = new AuthResponse();
			try
			{
				authResponse = await this.accountService.RefreshToken(request.UserId, request.RefreshToken);
			}
			catch (Exception e)
			{

				return this.BadRequest();
			}

			return this.Ok(authResponse);
		}

		[HttpGet]
		[Route("current")]
		[Authorize]
		public async Task<IActionResult> GetCurrentUser()
		{
			var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var result = await this.accountService.GetCurrentUser(userId);

			return this.Ok(result);
		}

		[HttpPost]
		[Route("updateUser")]
		[Authorize]
		public async Task<IActionResult>UpdateUser(UserUpdateModel model)
		{
			//check if admin or client
			var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			model.Id = userId;

			var result = await this.accountService.UpdateUser(model);	
			return this.Ok(result);
		}


		[HttpPost]
		[Route("changeThemeMode")]
		[Authorize]
		public async Task<IActionResult> ChangeThemeMode([FromBody]string mode = "light")
		{
			if (mode.ToLower() !="light" && mode.ToLower() != "dark")
			{
				return this.BadRequest("Invalid mode");
			}

			var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			await this.accountService.ChangeMode(mode, userId);

			return this.Ok();
		}

		[HttpPost]
		[Authorize(Roles ="ADMIN")]
		[Route("confirmUserEmail")]
		public async Task<IActionResult> ConfirmUserMail ([FromBody]string userId)
		{
			if (userId == null)
			{
				return this.BadRequest("Id is required");
			}
			await this.accountService.ConfirmUserEmail(userId);

			return this.Ok("Email Confirmed");
		}

	}
}
