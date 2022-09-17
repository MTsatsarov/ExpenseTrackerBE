using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StoreController : ControllerBase
	{
		private readonly IStoreService storeService;

		public StoreController(IStoreService storeService)
		{
			this.storeService = storeService;
		}

		[HttpGet]
		[Authorize]
		[Route("getStores")]
		public async Task<IActionResult> Get()
		{
			var result = await this.storeService.GetAllStores();
			return this.Ok(result);
		}

		[HttpPost]
		[Route("create")]
		[Authorize(Roles ="ADMIN")]
		public async Task<IActionResult> CreateStore(string name)
		{
			var isCreated = await this.storeService.CreateStore(name);
			if (!isCreated)
			{
				return this.BadRequest("Unable to create store.");
			}

			return this.Ok("Store created succesfully");
		}


		[HttpDelete]
		[Route("delete")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteStore(Guid id)
		{
			var isCreated = await this.storeService.DeleteStore(id);
			if (!isCreated)
			{
				return this.BadRequest("Unable to delete store.");
			}

			return this.Ok("Store deleted succesfully");
		}
	}
}
