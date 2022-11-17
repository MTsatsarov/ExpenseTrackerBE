using ExpenseTracker.Data;
using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.User;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
	public class OrganizationService : IOrganizationService
	{
		private readonly ExpenseTrackerDbContext db;
		private readonly IAccountService accountService;

		public OrganizationService(ExpenseTrackerDbContext db,IAccountService accountService)
		{
			this.db = db;
			this.accountService = accountService;
		}

		public async Task AddEmployee(RegisterEmployeeModel model, Organization organization)
		{
			await this.accountService.AddUser(model,organization);
		}

		public async Task<Organization> GetUserOrganization(string userId)
		{
			return await this.db.Organizations.FirstOrDefaultAsync(x => x.Owner == userId || x.Users.Any(x => x.Id == userId));
			
		}
	}
}
