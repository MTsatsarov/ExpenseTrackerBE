using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Products;
using ExpenseTracker.Services.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
	public class StorageService : IStorageService
	{
		private readonly IProductService productService;
		private readonly ExpenseTrackerDbContext db;
		private readonly IOrganizationService organizationService;

		public StorageService( ExpenseTrackerDbContext db,IOrganizationService organizationService,
			IProductService productService)
		{
			this.productService = productService;
			this.db = db;
			this.organizationService = organizationService;
		}
		public Task AddProduct(Product product)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<CompanyProducts>> GetStorage(string userId)
		{
			var organization = await this.organizationService.GetUserOrganization(userId);
			var products = new List<CompanyProducts>();

			var storages = 
				await this.db.Storages.Where(x=>x.OrganizationId == organization.Id).ToListAsync();

			foreach (var storage in storages)
			{
				string lastUpdate = storage.ModifiedOn != null ?
					storage.ModifiedOn.Value.ToString("dddd, dd MMMM yyyy") :
					storage.CreatedOn.ToString("dddd, dd MMMM yyyy");

				products.Add(new CompanyProducts()
				{
					Id = storage.Id,
					Product = storage.Product,
					Quantity = storage.Quantity,
					UpdatedBy = storage.UpdatedBy,
					LastUpdate = lastUpdate
				});
			}

			return products;
		}
	}
}
