using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Web
{
	[ApiController]
	[Route("api/[controller]")]
	public class StorageController : ControllerBase
	{
		private readonly IStorageService storageService;

		public StorageController(IStorageService storageService)
		{
			this.storageService = storageService;
		}

		[HttpGet]
		[Authorize]
		[Route("storage")]
		public async Task<IActionResult> GetStorage()
		{
			var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var products = await storageService.GetStorage(userId);

			return this.Ok(products);
		}

		[HttpPost]
		[Authorize]
		[Route("add")]
		public async Task<IActionResult> AddStorage(StorageInputModel model)
		{
			model.UserId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			await this.storageService.Add(model);

			return this.Ok();
		}

		[HttpPatch]
		[Authorize]
		[Route("update")]
		public async Task<IActionResult> Update(StorePatchModel model)
		{
			model.UserId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			await this.storageService.Update(model);

			return this.Ok();
		}
		
	}
}
