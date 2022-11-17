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

		[Required]
		public string Type { get; set; }

		public Guid? StoreId { get; set; }

		public string StoreName { get; set; }

		public Guid? ServiceId { get; set; }

		public string ServiceName { get; set; }

		public decimal? ServicePrice { get; set; }
	}
}
