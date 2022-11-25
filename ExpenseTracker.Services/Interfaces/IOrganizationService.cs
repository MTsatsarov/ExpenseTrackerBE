using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Models.Organization;
using ExpenseTracker.Services.Models.User;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IOrganizationService
	{
		 Task<Organization> GetUserOrganization(string userId);

		 Task AddEmployee(RegisterEmployeeModel model, Organization organization);

		Task<OrganizationUserResponse> GetAllUsers(Organization organization,int page,int itemsPerPage);

		IEnumerable<CurrenciesList> GetAllCurrencies();
	}
}
