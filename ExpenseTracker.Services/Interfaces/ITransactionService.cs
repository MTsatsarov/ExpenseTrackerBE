using ExpenseTracker.Services.Models.Transactions;

namespace ExpenseTracker.Services.Interfaces
{
	public interface ITransactionService
	{
		Task<bool> Create(TransactionInputModel model);
		Task<TransactionResponse> GetTransactions(string userId,int page,int itemsPerPage);
		Task<TransactionDetails> GetDetails(Guid transactionId);

		Task<DashboardTransactionsResponse> GetDashboardTransactions(string userId);
	}
}
