using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Models.Products;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IStorageService
	{
		Task AddProduct(Product product);

		Task<IEnumerable<CompanyProducts>> GetStorage(string userId);
	}
}
