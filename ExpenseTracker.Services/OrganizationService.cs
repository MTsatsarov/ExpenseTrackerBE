using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Organization;
using ExpenseTracker.Services.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System;

namespace ExpenseTracker.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly ExpenseTrackerDbContext db;
		private readonly IAccountService accountService;

		public OrganizationService(ExpenseTrackerDbContext db, IAccountService accountService)
		{
			this.db = db;
			this.accountService = accountService;
		}

		public async Task AddEmployee(RegisterEmployeeModel model, Organization organization)
		{
			await this.accountService.AddUser(model, organization);
		}

		public IEnumerable<CurrenciesList> GetAllCurrencies()
		{
			var currencies = new List<CurrenciesList>();
			var solutiondir = Path.Combine(Environment.CurrentDirectory, "../ExpenseTracker.Services/StaticFiles/Currencies.json");
			using (StreamReader r = new StreamReader(solutiondir))
			{
				string json = r.ReadToEnd();
				currencies = JsonSerializer.Deserialize<List<CurrenciesList>>(json);
			}

			return currencies;
		}

		public async Task<OrganizationUserResponse> GetAllUsers
			(Organization organization,int page,int itemsPerPage )
		{
			var organizationResponse = new OrganizationUserResponse();
			var list = new List<OrganizationUserList>();

			var users = await this.db.Users.Where(x => x.OrganizationId == organization.Id).ToListAsync();
			organizationResponse.Count = users.Count;
			var filteredUsers = users.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();

			foreach (var user in filteredUsers)
			{
				var employee = new OrganizationUserList();
				employee.Id = user.Id;
				employee.Email = user.Email;
				employee.CreatedOn = user.CreatedOn.ToString("MM/dd/yyyy h:mmtt");

				var userExpense = user.Expenses.OrderByDescending(x => x.ExpenseProducts.Sum(x => x.Price * x.Quantity)).FirstOrDefault();

				if (userExpense is not null)
				{
					employee.HighestSum = userExpense.ExpenseProducts.FirstOrDefault().Price;
					employee.LastTransaction = user.Expenses.OrderByDescending(x => x.CreatedOn).FirstOrDefault().CreatedOn.ToString("MM/dd/yyyy h:mm tt");
				}
				employee.TotalSum = user.Expenses.Sum(x => x.ExpenseProducts.Sum(x => x.Price * x.Quantity));
				employee.TotalTransactions = user.Expenses.ToList().Count();
				employee.UserName = user.UserName;
				list.Add(employee);
			}

			organizationResponse.Employees = list.ToList();
			return organizationResponse;
		}

		public async Task<Organization> GetUserOrganization(string userId)
		{
			return await this.db.Organizations.FirstOrDefaultAsync(x => x.Owner == userId || x.Users.Any(x => x.Id == userId));

		}
	}
}
