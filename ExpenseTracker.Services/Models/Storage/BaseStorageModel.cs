using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Services.Models.Storage
{
	public abstract class BaseStorageModel
	{
		public string UserId { get; set; }

		[Required]
		public string Product { get; set; }

		public decimal Quantity { get; set; }
	}
}
