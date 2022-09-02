using ExpenseTracker.Services.Models.Transactions;

namespace ExpenseTracker.Services.Interfaces
{
	public interface ITransactionService
	{
		Task<bool> Create(TransactionInputModel model);
	}
}
