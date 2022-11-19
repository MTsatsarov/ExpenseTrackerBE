using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Organization;
using ExpenseTracker.Services.Models.User;
using Microsoft.EntityFrameworkCore;

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

		public async Task<IEnumerable<OrganizationUserList>> GetAllUsers(Organization organization)
		{
			var list = new List<OrganizationUserList>();
			var users = await this.db.Users.Where(x => x.OrganizationId == organization.Id).ToListAsync();

			foreach (var user in users)
			{
				var a = new OrganizationUserList();
				a.Id = user.Id;
				a.Email = user.Email;
				a.CreatedOn = user.CreatedOn.ToString("MM/dd/yyyy h:mmtt");

				var s = user.Expenses.OrderByDescending(x => x.ExpenseProducts.OrderByDescending(x => x.Price * x.Quantity)).FirstOrDefault();

				if (s is not null)
				{
					a.HighestSum = s.ExpenseProducts.FirstOrDefault().Price;
					a.LastTransaction = user.Expenses.OrderByDescending(x => x.CreatedOn).FirstOrDefault().CreatedOn.ToString("MM/dd/yyyy h:mm tt");

				}
				a.TotalSum = user.Expenses.Sum(x => x.ExpenseProducts.Sum(x => x.Price * x.Quantity));
				a.TotalTransactions = user.Expenses.ToList().Count();
				a.UserName = user.UserName;
				list.Add(a);
			}

			return list;
		}

		public async Task<Organization> GetUserOrganization(string userId)
		{
			return await this.db.Organizations.FirstOrDefaultAsync(x => x.Owner == userId || x.Users.Any(x => x.Id == userId));

		}
	}
}
