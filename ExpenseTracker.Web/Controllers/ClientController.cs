using ExpenseTracker.Common;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Authorize(Roles ="ADMIN")]
	public class ClientController : ControllerBase
	{
		private readonly IClientService clientService;

		public ClientController(IClientService clientService)
		{
			this.clientService = clientService;
		}

		[HttpGet]
		[Route("all")]
		public async Task<IActionResult> All()
		{
			var response = await this.clientService.GetAll();
			return this.Ok(response);
		}

		[HttpGet]
		[Authorize(Roles ="ADMIN")]
		[Route("byId")]
		public async Task<IActionResult> GetById(string userId)
		{
			var response = await this.clientService.GetById(userId);

			return this.Ok(response);
		}
	}
}
