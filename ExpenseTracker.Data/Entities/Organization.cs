using ExpenseTracker.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Entities
{
	public class Organization : BaseDeletableEntity<Guid>
	{
		public Organization()
		{
			this.Id = Guid.NewGuid();
			this.Users = new HashSet<ApplicationUser>();
			this.Expenses = new HashSet<Expense>();
		}

		[Required]
		[StringLength(50, MinimumLength = 2)]
		public string Name { get; set; }

		public string Owner { get; set; }

		[Required]
		public string Currency { get; set; }

		[Required]
		public string CurrencySymbol { get; set; }

		[Required]
		public string Abbreviation { get; set; }

		public virtual ICollection<Expense> Expenses { get; set; }

		[Required]
		public virtual ICollection<ApplicationUser> Users { get; set; }
	}
}
