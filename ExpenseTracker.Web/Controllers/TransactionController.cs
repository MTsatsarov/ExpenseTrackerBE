using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TransactionController : ControllerBase
	{
		private readonly ITransactionService transactionService;

		public TransactionController(ITransactionService transactionService)
		{
			this.transactionService = transactionService;
		}

		[HttpPost]
		[Route("create")]
		[Authorize]
		public async Task<IActionResult> AddTransaction(TransactionInputModel model)
		{
			if (model.UserId == null)
			{
				model.UserId = this.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
			}

			var result = await this.transactionService.Create(model);

			if (!result)
			{
				return this.BadRequest("Transaction not created");
			}

			return this.Ok("Transaction created succesfully.");
		}

		[HttpGet]
		[Route("getDashboardTransactions")]
		[Authorize(Roles ="CLIENT")]
		public async Task<IActionResult> GetDashboardTransactions()
		{
			var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
			var result = await this.transactionService.GetDashboardTransactions(userId);
			return this.Ok(result);
		}

		[HttpGet]
		[Route("getTransactions")]
		[Authorize(Roles = "CLIENT")]
		public async Task<IActionResult> GetTransactions()
		{
			var userId = this.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			if (string.IsNullOrEmpty(userId))
			{
				return this.BadRequest("User id missing.");
			}
			var result = await this.transactionService.GetTransactions(userId);
			return this.Ok(result);
		}

		[HttpGet]
		[Route("getUserTransactions")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> GetUserTransactions()
		{
			// WHY USERID is null ????
			var result = await this.transactionService.GetTransactions(null);
			return this.Ok(result);
		}

		[HttpGet]
		[Route("details/{id}")]
		[Authorize]
		public async Task<IActionResult> GetDetails(Guid id)
		{
			var result = await this.transactionService.GetDetails(id);

			return this.Ok(result);
		}
	}
}
