
using ExpenseTracker.Data.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Data.Entities
{
	public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
	{
		public ApplicationUser()
		{
			this.Id = Guid.NewGuid().ToString();
			this.Expenses = new HashSet<Expense>();
		}

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		public long Budget { get; set; }

		public bool IsDeleted { get; set; }

		public DateTime? DeletedOn { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }

		public Guid OrganizationId { get; set; }

		public virtual Organization Organization { get; set; }

		[ForeignKey("Settings")]
		public Guid SettingsId { get; set; }

		public virtual Settings Settings{ get; set; }

		public virtual ICollection<Expense> Expenses { get; set; }

		public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

		public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

		public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

	}
}
