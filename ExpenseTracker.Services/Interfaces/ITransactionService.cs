using ExpenseTracker.Services.Models.Transactions;

namespace ExpenseTracker.Services.Interfaces
{
	public interface ITransactionService
	{
		Task<bool> Create(TransactionInputModel model);
		Task<List<TransactionResponse>> GetTransactions(string userId);
		Task<TransactionDetails> GetDetails(Guid transactionId);

		Task<DashboardTransactionsResponse> GetDashboardTransactions(string userId);
	}
}
