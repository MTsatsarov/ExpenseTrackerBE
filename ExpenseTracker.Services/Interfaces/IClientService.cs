
using ExpenseTracker.Services.Models.Client;
using ExpenseTracker.Services.Models.User;
using Microsoft.AspNet.Identity;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IClientService
	{
		Task<ClientListResponse> GetAll();
		Task<UserInfo> GetById(string id);
	}
}
