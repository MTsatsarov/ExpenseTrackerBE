
namespace ExpenseTracker.Services.Models.Transactions
{
	public class GetTransactionModel
	{
		public string UserId { get; set; }

		public int Page { get; set; }

		public int ItemsPerPage { get; set; }

		public int? Day { get; set; }
	}
}
