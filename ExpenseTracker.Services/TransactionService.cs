using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Transactions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
	public class TransactionService : ITransactionService
	{
		private readonly ExpenseTrackerDbContext db;
		private readonly IStoreService storeService;
		private readonly IProductService productService;

		public TransactionService(ExpenseTrackerDbContext db, IStoreService storeService, IProductService productService)
		{
			this.db = db;
			this.storeService = storeService;
			this.productService = productService;
		}

		public async Task<bool> Create(TransactionInputModel model)
		{
			var transaction = new Expense();
			if (model.StoreId == null)
			{
				await this.storeService.CreateStore(model.StoreName);
			}
			//Refactor
			var store = await this.db.Stores.FirstOrDefaultAsync(x => x.Name == model.StoreName);

			foreach (var product in model.Products)
			{
				var currentProduct = new Product();
				if (product.ProductId != null)
				{
					currentProduct = await this.db.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId.Value);
					if (currentProduct == null)
					{
						throw new InvalidOperationException("Invalid product id ");
					}
				}
				else
				{
					currentProduct.Name = product.Name;
					currentProduct.Stores.Add(store);
					await this.db.Products.AddAsync(currentProduct);
				}

				var expenseProduct = new ExpenseProducts()
				{
					Price = product.Price,
					Quantity = product.Quantity,
					ProductId = currentProduct.Id,
				};

				transaction.ExpenseProducts.Add(expenseProduct);
			}

			await this.db.Expenses.AddAsync(transaction);
			var result = await this.db.SaveChangesAsync();
			return result > 0;
		}
	}
}
