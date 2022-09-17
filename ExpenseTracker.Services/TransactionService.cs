﻿using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Products;
using ExpenseTracker.Services.Models.Transactions;
using ExpenseTracker.Services.Utils.Exceptions;
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
						throw new BadRequestException("Invalid product");
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
			var response = new  DashboardTransactionsResponse();

			var yearlyTransactions =await this.db.Expenses.Where(x => x.CreatedOn.Year == currentYear && x.UserId == userId).ToListAsync();

			var currentMonthTransactions = yearlyTransactions.Where(x => x.CreatedOn.Month == currentMonth && x.CreatedOn.Year == currentYear);

			var transactionsByDate = currentMonthTransactions.Select(x => new TransactionsByDate()
			{
				Day = x.CreatedOn.Day,
				TotalSum = x.ExpenseProducts.Sum(ep => (ep.Quantity * ep.Price))
			}).ToList();

			var storeTransactions = currentMonthTransactions.Select(x => new TransactionsByStore()
			{
				
			}).ToList();

			var transactionsByMonth =  yearlyTransactions.GroupBy(x => x.CreatedOn.Month).ToList();

			;
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
			var transactions = this.db.Expenses;
			var result = new List<TransactionResponse>();

			if (!string.IsNullOrEmpty(userId))
			{
				transactions.Where(x => x.UserId == userId);
			}

			var data = await transactions.ToListAsync();

			foreach (var transaction in data)
			{
				var a = (new TransactionResponse()
				{
					CreatedOn = transaction.CreatedOn,
					Id = transaction.Id,
					//Store = transaction.Stores.FirstOrDefault().Name,
					Store = "My store",
					TotalPrice = transaction.ExpenseProducts.Sum(x => x.Price * x.Quantity)
				});
				result.Add(a);
			}

			return result;
		}
	}
}