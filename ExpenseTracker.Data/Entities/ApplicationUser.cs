
using ExpenseTracker.Data.Common;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Data.Entities
{
	public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
	{
		public ApplicationUser()
		{
			this.Id = Guid.NewGuid().ToString();
		}

		public long Budget { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }

		public virtual ICollection<Expense> Expenses { get; set; }

		public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

		public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

		public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

	}
}
