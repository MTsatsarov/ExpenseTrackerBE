using ExpenseTracker.Common;
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

		public TransactionService(ExpenseTrackerDbContext db,
			IStoreService storeService,
			IProductService productService)
		{
			this.db = db;
			this.storeService = storeService;
			this.productService = productService;
		}

		public async Task<bool> Create(TransactionInputModel model)
		{
			var transaction = new Expense();

			transaction.Organization =
				await this.db.Organizations.FirstOrDefaultAsync(o => o.Users.Any(x => x.Id == model.UserId));

			var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
			if (user is null)
			{
				throw new BadRequestException("Invalid user.");
			}

			if (transaction.Organization is null)
			{
				throw new BadRequestException("Something went wrong.... Please contact your administrator.");
			}

			if (model.Type == ExpenseTypeConstants.Product)
			{
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

					if (product.ProductId is not null)
					{
						currentProduct =
							await this.db.Products.FirstOrDefaultAsync(x => x.Id == product.ProductId.Value);
						if (currentProduct is null)
						{
							throw new BadRequestException("Invalid product");
						}
					}
					else
					{
						currentProduct = await this.db.Products.FirstOrDefaultAsync(x => x.Name == product.Name);
						if (currentProduct is null)
						{
							currentProduct = new Product();
							currentProduct.Name = product.Name;
							currentProduct.Stores.Add(store);
							await this.db.Products.AddAsync(currentProduct);
							await this.db.SaveChangesAsync();
						}

					}

					var expenseProduct = new ExpenseProducts()
					{
						Price = product.Price,
						Quantity = product.Quantity,
						ProductId = currentProduct.Id,
					};

					transaction.ExpenseProducts.Add(expenseProduct);
					transaction.UserId = model.UserId;

					var storage = await this.db.Storages
						.FirstOrDefaultAsync(x => x.Organization.Id == transaction.Organization.Id &&
							x.Product == currentProduct.Name);

					if (storage is null)
					{
						storage = new Storage()
						{
							Product = currentProduct.Name,
							Quantity = product.Quantity,
							UpdatedBy = user.Email,
							OrganizationId = transaction.Organization.Id
						};

						await this.db.AddAsync(storage);
					}
					else
					{
						storage.ModifiedOn = DateTime.UtcNow;
						storage.Quantity = product.Quantity += product.Quantity;
						storage.UpdatedBy = user.Email;
					}

				}
			}
			else if (model.Type == ExpenseTypeConstants.Service)
			{
				var service = new Service();

				if (model.ServiceId == null)
				{
					service.Name = model.ServiceName;

				}
				else
				{
					service = await this.db.Services.FirstOrDefaultAsync(x => x.Id == model.ServiceId);
				}

				var expenseService = new ExpenseServices()
				{
					Service = service,
					Price = model.ServicePrice.Value,
				};

				transaction.ExpenseService = expenseService;
			}
			else
			{
				throw new BadRequestException("Invalid type of the expense");
			}

			transaction.Type = model.Type;



			await this.db.Expenses.AddAsync(transaction);
			var result = await this.db.SaveChangesAsync();
			return result > 0;
		}

		public async Task<DashboardTransactionsResponse> GetDashboardTransactions(string userId)
		{
			var currentMonth = DateTime.UtcNow.Month;
			var currentYear = DateTime.UtcNow.Year;
			var response = new DashboardTransactionsResponse();
			var organization = await this.db.Organizations.FirstOrDefaultAsync(x => x.Users.Any(x => x.Id == userId));

			if (organization is null)
			{
				throw new BadRequestException("Something went wrong.");
			}

			var yearlyTransactions = await this.db.Expenses.Where(x => x.CreatedOn.Year == currentYear &&
			x.Organization == organization).ToListAsync();

			foreach (var transaction in yearlyTransactions)
			{
				var currenTransaction = response.LastYearTransactions.FirstOrDefault(x => x.Month == transaction.CreatedOn.ToString("MMMM", CultureInfo.InvariantCulture));
				if (currenTransaction != null)
				{
					if (transaction.Type == ExpenseTypeConstants.Product)
					{
						currenTransaction.Sum += transaction.ExpenseProducts.Sum(ep => (ep.Quantity * ep.Price));
					}
					else
					{
						currenTransaction.Sum += transaction.ExpenseService.Price;
					}
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

				if (transaction.Type == ExpenseTypeConstants.Product)
				{
					currentTransaction.Sum = transaction.ExpenseProducts.Sum(ep => (ep.Quantity * ep.Price));
				}
				else
				{
					currentTransaction.Sum = transaction.ExpenseService.Price;
				}

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

		public async Task<TransactionResponse> GetTransactions(GetTransactionModel model)
		{
			var organization = await this.db.Organizations
				.FirstOrDefaultAsync(x => x.Users.Any(x => x.Id == model.UserId));

			var transactions = await this.db.Expenses
				.Where(x => x.Organization == organization).ToListAsync();
				
			if(model.Day is not null)
			{
				transactions =
					transactions.Where(x => x.CreatedOn.Month == DateTime.UtcNow.Month && x.CreatedOn.Day == model.Day).ToList();
			}


			var filteredTransactions = transactions
				.Skip(model.ItemsPerPage * (model.Page - 1))
				.Take(model.ItemsPerPage)
				.ToList();

			var result = new TransactionResponse();
			var responseTransactions = new List<HistoryTransactions>();

			foreach (var transaction in filteredTransactions)
			{
				var currTransaction = (new HistoryTransactions()
				{
					CreatedOn = transaction.CreatedOn.ToString("dddd, dd MMMM yyyy HH:mm:ss",CultureInfo.InvariantCulture),
					Id = transaction.Id,
					Store = transaction.Stores.FirstOrDefault().Name,
					User = transaction.User.Email,
				});

				if (transaction.Type == ExpenseTypeConstants.Product)
				{
					currTransaction.TotalPrice = transaction.ExpenseProducts.Sum(x => x.Price * x.Quantity);
				}
				else
				{
					currTransaction.TotalPrice = transaction.ExpenseService.Price;
				}
				responseTransactions.Add(currTransaction);
			}
			result.Transactions = responseTransactions;
			result.Count = transactions.Count();

			return result;
		}
	}
}
