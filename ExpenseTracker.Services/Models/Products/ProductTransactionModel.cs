
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.Products
{
	public class ProductTransactionModel
	{
		public Guid? ProductId { get; set; }

		[Required]
		public string Name{ get; set; }

		public decimal Price { get; set; }

		public int Quantity { get; set; }
	}
}
