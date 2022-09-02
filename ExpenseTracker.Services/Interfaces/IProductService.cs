
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.Products;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IProductService
	{
		Task<IEnumerable<ProductResponse>> GetProducts(string name);
		Task<bool> CreateProduct(ProductInputModel model);
		Task<bool> DeleteProduct(Guid productId);
	}
}
