using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Products;
using ExpenseTracker.Services.Models.Storage;
using ExpenseTracker.Services.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.Services
{
	public class StorageService : IStorageService
	{
		private readonly IProductService productService;
		private readonly ExpenseTrackerDbContext db;
		private readonly IOrganizationService organizationService;

		public StorageService(ExpenseTrackerDbContext db, IOrganizationService organizationService,
			IProductService productService)
		{
			this.productService = productService;
			this.db = db;
			this.organizationService = organizationService;
		}

		public async Task Add(StorageInputModel model)
		{
			var organization = await this.organizationService.GetUserOrganization(model.UserId);
			var storage = new Storage()
			{
				Product = model.Product,
				OrganizationId = organization.Id,
				Quantity = model.Quantity,
				UpdatedBy = model.Email,

			};

			await this.db.Storages.AddAsync(storage);
			await this.db.SaveChangesAsync();

		}

		public async Task<StorageResponse> GetStorage(int page, int itemsPerPage ,string userId)
		{
			var organization = await this.organizationService.GetUserOrganization(userId);
			var response = new StorageResponse();
			var products = new List<CompanyProducts>();

			var storages =
				await this.db.Storages.Where(x => x.OrganizationId == organization.Id)
				.OrderByDescending(x=>x.CreatedOn)
				.ToListAsync();
			response.Count = storages.Count;

			var filteredStorages = storages.Skip(itemsPerPage * (page - 1)).Take(itemsPerPage).ToList();

			foreach (var storage in filteredStorages)
			{
				string lastUpdate = storage.ModifiedOn != null ?
					storage.ModifiedOn.Value.ToString("dddd, dd MMMM yyyy",CultureInfo.InvariantCulture) :
					storage.CreatedOn.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture);

				products.Add(new CompanyProducts()
				{
					Id = storage.Id,
					Product = storage.Product,
					Quantity = storage.Quantity,
					UpdatedBy = storage.UpdatedBy,
					LastUpdate = lastUpdate
				});
			}
			response.Products = products;
			return response;
		}

		public async Task Update(StorePatchModel model)
		{
			var organization = await this.organizationService.GetUserOrganization(model.UserId);
			var storage = organization.Storages.FirstOrDefault(x => x.Id == model.StorageId);

			if (storage is null)
			{
				throw new BadRequestException("Invalid storage.");
			}

			storage.Product = model.Product;
			storage.Quantity = model.Quantity;

			await this.db.SaveChangesAsync();

		}
	}
}
