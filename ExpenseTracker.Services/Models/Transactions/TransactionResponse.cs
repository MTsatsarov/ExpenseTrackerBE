namespace ExpenseTracker.Services.Models.Transactions
{
	public class TransactionResponse
	{
		public IEnumerable<HistoryTransactions> Transactions { get; set; }

		public int Count { get; set; }
	}

	public class HistoryTransactions
	{
		public Guid Id { get; set; }

		public decimal TotalPrice { get; set; }

		public string Store { get; set; }

		public string CreatedOn { get; set; }

		public string User { get; set; }
	}
}
