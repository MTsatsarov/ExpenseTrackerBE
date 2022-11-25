using ExpenseTracker.Services.Models.Storage;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IStorageService
	{
		Task Add(StorageInputModel model);

		Task<StorageResponse> GetStorage(int page,int itemsPerPage, string userId);

		Task Update(StorePatchModel model);
	}
}
