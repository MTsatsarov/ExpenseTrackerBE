using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ExpenseTracker.Web.Controllers
{
	[ApiController]
	[Authorize]
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
	}
}
