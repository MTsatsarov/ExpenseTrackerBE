
using ExpenseTracker.Services.Models.Products;

namespace ExpenseTracker.Services.Models.Transactions
{
	public class TransactionDetails
	{
		public TransactionDetails()
		{
			Products = new List<ProductTransactionModel>();
		}
		public Guid Id { get; set; }

		public string Store { get; set; }

		public List<ProductTransactionModel> Products { get; set; }
	}
}
