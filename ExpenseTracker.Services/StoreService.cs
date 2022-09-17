
using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Store;
using ExpenseTracker.Services.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
	public class StoreService : IStoreService
	{
		private readonly ExpenseTrackerDbContext db;

		public StoreService(ExpenseTrackerDbContext db)
		{
			this.db = db;
		}

		public async Task<bool> CreateStore(string name)
		{
			var store = await this.db.Stores.FirstOrDefaultAsync(x => x.Name == name);
			if (store != null)
			{
				throw new BadRequestException("Store name is already taken.");
			}

			store = new Store()
			{
				Name = name,
				//TODO add address
			};

			await this.db.Stores.AddAsync(store);
			var result = await this.db.SaveChangesAsync();

			return result > 0;
		}

		public async Task<bool> DeleteStore(Guid storeId)
		{
			var store = await this.db.Stores.FirstOrDefaultAsync(x => x.Id == storeId);
			if (store == null)
			{
				throw new BadRequestException("Store not found.");
			}

			store.IsDeleted = true;

			this.db.Stores.Update(store);
			var result = await this.db.SaveChangesAsync();

			return result > 0;
		}

		public async Task<IEnumerable<StoreResponseModel>> GetAllStores()
		{
			var stores = await this.db.Stores.ToListAsync();
			var storeResponse = new List<StoreResponseModel>();

			foreach (var store in stores)
			{
				storeResponse.Add(new StoreResponseModel()
				{
					StoreId = store.Id,
					Name = store.Name,
				});
			}
			return storeResponse.OrderBy(x=>x.Name);
		}
	}
}
