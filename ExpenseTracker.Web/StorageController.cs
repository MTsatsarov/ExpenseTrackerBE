using ExpenseTracker.Services.Interfaces;
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
	}
}
