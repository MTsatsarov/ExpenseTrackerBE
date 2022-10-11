using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Products;
using ExpenseTracker.Services.Models.Transactions;
using ExpenseTracker.Services.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;

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

			if (store == null)
			{
				throw new BadRequestException("Invalid store");
			}

			transaction.Stores.Add(store);

			foreach (var product in model.Products)
			{
				var currentProduct = new Product();
				//TODO REMOVE MULTIPLE ADDING OF SAME PRODUCT
				if (product.ProductId != null)
				{
					currentProduct = await this.db.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId.Value);
					if (currentProduct == null)
					{
						throw new BadRequestException("Invalid product");
					}
				}
				else
				{

					currentProduct.Name = product.Name;
					currentProduct.Stores.Add(store);
					await this.db.Products.AddAsync(currentProduct);
					await this.db.SaveChangesAsync();
				}

				var expenseProduct = new ExpenseProducts()
				{
					Price = product.Price,
					Quantity = product.Quantity,
					ProductId = currentProduct.Id,
				};

				transaction.ExpenseProducts.Add(expenseProduct);
				transaction.UserId = model.UserId;
			}

			await this.db.Expenses.AddAsync(transaction);
			var result = await this.db.SaveChangesAsync();
			return result > 0;
		}

		public async Task<DashboardTransactionsResponse> GetDashboardTransactions(string userId)
		{
			var currentMonth = DateTime.UtcNow.Month;
			var currentYear = DateTime.UtcNow.Year;
			var response = new DashboardTransactionsResponse();

			var yearlyTransactions = await this.db.Expenses.Where(x => x.CreatedOn.Year == currentYear && x.UserId == userId).ToListAsync();
			foreach (var transaction in yearlyTransactions)
			{
				var currenTransaction = response.LastYearTransactions.FirstOrDefault(x => x.Month == transaction.CreatedOn.ToString("MMMM", CultureInfo.InvariantCulture));
				if (currenTransaction != null)
				{
					currenTransaction.Sum += transaction.ExpenseProducts.Sum(ep => (ep.Quantity * ep.Price));
				}
				
			}

			var currentMonthTransactions = yearlyTransactions.Where(x => x.CreatedOn.Month == currentMonth).ToList();

			for (int i = 1; i <= 31; i++)
			{
				response.CurrentMonthTransactions.Add(new TransactionsByDate()
				{
					Name = i.ToString(),
				});
			}

			foreach (var transaction in currentMonthTransactions)
			{
				var currentTransaction = response.CurrentMonthTransactions.FirstOrDefault(x => int.Parse(x.Name) == transaction.CreatedOn.Day);
				currentTransaction.Sum = transaction.ExpenseProducts.Sum(ep => (ep.Quantity * ep.Price));
			}

			var storeTransactions = new Dictionary<string, int>();

			foreach (var expense in yearlyTransactions)
			{
				var currStore = expense.Stores.FirstOrDefault().Name;

				if (!storeTransactions.ContainsKey(currStore))
				{
					storeTransactions.Add(currStore, 0);
				}
				storeTransactions[currStore] += 1;
			}
			var transactions = storeTransactions.OrderByDescending(x => x.Value).Take(5).ToDictionary(x => x.Key, x => x.Value);

			foreach (var transaction in transactions)
			{
				response.TransactionsByStore.Add(new TransactionByStore()
				{
					Name = transaction.Key,
					Count = transaction.Value,
				});
			}

			return response;
		}

		public async Task<TransactionDetails> GetDetails(Guid id)
		{
			var transaction = await this.db.Expenses.Where(x => x.Id == id).FirstOrDefaultAsync();

			if (transaction == null)
			{
				throw new BadRequestException("Transaction not found.");
			}
			var transactionModel = new TransactionDetails();
			transactionModel.Id = id;
			transactionModel.Store = transaction.Stores.FirstOrDefault().Name;

			foreach (var product in transaction.ExpenseProducts)
			{
				transactionModel.Products.Add(new ProductTransactionModel()
				{
					Name = product.Product.Name,
					Price = product.Price,
					ProductId = product.ProductId,
					Quantity = product.Quantity
				});
			}

			return transactionModel;
		}

		public async Task<List<TransactionResponse>> GetTransactions(string userId)
		{
			var transactions = await this.db.Expenses.Where(x => x.UserId == userId).ToListAsync();
			var result = new List<TransactionResponse>();

			foreach (var transaction in transactions)
			{
				var a = (new TransactionResponse()
				{
					CreatedOn = transaction.CreatedOn,
					Id = transaction.Id,
					Store = transaction.Stores.FirstOrDefault().Name,
					TotalPrice = transaction.ExpenseProducts.Sum(x => x.Price * x.Quantity)
				});
				result.Add(a);
			}

			return result;
		}
	}
}
