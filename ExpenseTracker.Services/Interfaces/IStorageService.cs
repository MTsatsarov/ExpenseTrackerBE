using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Models.Products;
using ExpenseTracker.Services.Models.Storage;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IStorageService
	{
		Task Add(StorageInputModel model);

		Task<IEnumerable<CompanyProducts>> GetStorage(string userId);

		Task Update(StorePatchModel model);
	}
}
