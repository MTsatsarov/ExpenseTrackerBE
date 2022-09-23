using ExpenseTracker.Services.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.Transactions
{
	public class TransactionInputModel
	{
		public TransactionInputModel()
		{
			this.Products = new HashSet<ProductTransactionModel>();
		}
		public string UserId { get; set; }
		public ICollection<ProductTransactionModel> Products { get; set; }

		public Guid? StoreId { get; set; }

		[Required]
		public string StoreName { get; set; }
	}
}
