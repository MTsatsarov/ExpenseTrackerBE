using ExpenseTracker.Common;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrganizationController:ControllerBase
	{
		private readonly IOrganizationService organizationService;

		public OrganizationController(IOrganizationService organizationService)
		{
			this.organizationService = organizationService;
		}

		[HttpPost]
		[Route("addEmployee")]
		[Authorize(Roles = "OWNER")]
		public async Task<IActionResult> AddEmployee(RegisterEmployeeModel model)
		{
			var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var organization = await this.organizationService.GetUserOrganization(userId);
			await this.organizationService.AddEmployee(model, organization);

			return this.Ok();
		}

	}
}
