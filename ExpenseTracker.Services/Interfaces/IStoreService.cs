
using ExpenseTracker.Services.Models.Store;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IStoreService
	{
		Task<IEnumerable<StoreResponseModel>> GetAllStores();

		Task<bool> CreateStore(string name);

		Task<bool> DeleteStore(Guid storeId);
	}
}
