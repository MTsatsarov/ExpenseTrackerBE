using ExpenseTracker.Data;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Services.Models.Client;
using ExpenseTracker.Services.Models.User;
using ExpenseTracker.Services.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ExpenseTracker.Services
{
	public class ClientService : IClientService
	{
		private readonly ExpenseTrackerDbContext db;
		private readonly IAccountService accountService;

		public ClientService(ExpenseTrackerDbContext db, IAccountService accountService)
		{
			this.db = db;
			this.accountService = accountService;
		}

		public async Task<ClientListResponse> GetAll()
		{
			var response = new ClientListResponse();
			var users = await this.accountService.GetAllUsers();

			var list = new List<ClientList>();
			foreach (var user in users)
			{
				list.Add(new ClientList()
				{
					Email = user.Email,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Id = user.Id,
					Organization = user.Organization.Name,
					TotalTransactions = user.Expenses.Count(),
					CreatedOn = user.CreatedOn.ToString("dddd, dd MMMM yyyy", CultureInfo.InvariantCulture)
				}) ;
			}
			response.Clients = list.ToList();

			return response;
		}

		public async Task<UserInfo> GetById(string id)
		{
			if(id == null)
			{
				throw new BadRequestException("Invalid id");
			}

			var user = await this.db.Users.FirstOrDefaultAsync(x => x.Id == id);

			if (user is null)
			{
				throw new BadRequestException("User not found");
			}

			var userInfo = new UserInfo(user);

			return userInfo;
		}
	}
}
