﻿
using ExpenseTracker.Data.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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

		public virtual ICollection<Expense> Expenses { get; set; }

		public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

		public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

		public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

	}
}
