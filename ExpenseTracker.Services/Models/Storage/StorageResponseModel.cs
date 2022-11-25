using ExpenseTracker.Services.Models.Products;

namespace ExpenseTracker.Services.Models.Storage
{
	public class StorageResponse
	{
		public int Count { get; set; }

		public int Page { get; set; }
		public IEnumerable<CompanyProducts> Products { get; set; }
	}
}
