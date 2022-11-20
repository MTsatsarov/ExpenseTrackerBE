using ExpenseTracker.Data.Entities;
using ExpenseTracker.Services.Models.Organization;
using ExpenseTracker.Services.Models.User;

namespace ExpenseTracker.Services.Interfaces
{
	public interface IOrganizationService
	{
		 Task<Organization> GetUserOrganization(string userId);

		 Task AddEmployee(RegisterEmployeeModel model, Organization organization);

		Task<IEnumerable<OrganizationUserList>> GetAllUsers(Organization organization);

		IEnumerable<CurrenciesList> GetAllCurrencies();
	}
}
