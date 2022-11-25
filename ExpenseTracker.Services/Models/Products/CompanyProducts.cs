namespace ExpenseTracker.Services.Models.Products
{
	public class CompanyProducts
	{
		public Guid Id { get; set; }

		public string Product { get; set; }

		public decimal Quantity { get; set; }

		public string LastUpdate { get; set; }

		public string UpdatedBy { get; set; }
	}
}
