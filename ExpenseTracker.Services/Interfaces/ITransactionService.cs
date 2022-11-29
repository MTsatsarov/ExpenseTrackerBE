using ExpenseTracker.Services.Models.Transactions;

namespace ExpenseTracker.Services.Interfaces
{
	public interface ITransactionService
	{
		Task<bool> Create(TransactionInputModel model);
		Task<TransactionResponse> GetTransactions(GetTransactionModel model);
		Task<TransactionDetails> GetDetails(Guid transactionId);

		Task<DashboardTransactionsResponse> GetDashboardTransactions(string userId);
	}
}
