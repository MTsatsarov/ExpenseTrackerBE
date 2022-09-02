using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models;
using ExpenseTracker.Services.Models.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ExpenseTracker.Services
{
	public class ProductService : IProductService
	{
		private readonly ExpenseTrackerDbContext db;

		public ProductService(ExpenseTrackerDbContext db)
		{
			this.db = db;
		}

		public async Task<bool> CreateProduct(ProductInputModel model)
		{
			var product = await this.db.Products.FirstOrDefaultAsync(x => x.Name == model.Name);
			if (product != null)
			{
				throw new InvalidOperationException("Product already exists");
			}

			var store = await this.db.Stores.FirstOrDefaultAsync(x => x.Id == model.StoreId);
			if (store == null)
			{
				throw new InvalidOperationException("Store not found");
			}

			var productToAdd = new Product()
			{
				Name = model.Name,
				Stores = new List<Store>() { store }
			};

			await this.db.Products.AddAsync(productToAdd);
			var result = await this.db.SaveChangesAsync();

			return result > 0;

		}

		public async Task<bool> DeleteProduct(Guid productId)
		{
			var product = await this.db.Products.FirstOrDefaultAsync(x => x.Id == productId);

			if (product == null)
			{
				throw new InvalidOperationException("Product not found.");
			}

			product.IsDeleted = true;
			this.db.Products.Update(product);
			var result = await this.db.SaveChangesAsync();

			return result > 0;
		}

		public async Task<IEnumerable<ProductResponse>> GetProducts(string name)
		{
			var responseProducts = new List<ProductResponse>();
			var products = this.db.Products.AsQueryable();

			if (name != null)
			{
				products.Where(x => x.Name.Contains(name));
			}
			products = (IQueryable<Product>)products.ToList();
			foreach (var product in products)
			{
				var currProduct = new ProductResponse()
				{
					Id = product.Id,
					Name = product.Name,
				};
				responseProducts.Add(currProduct);
			}

			return responseProducts;
		}
	}
}
